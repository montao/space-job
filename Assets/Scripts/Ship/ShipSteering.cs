using Unity.Netcode;
using UnityEngine;
using System;

[RequireComponent(typeof(ShipManager), typeof(NetworkObject))]
public class ShipSteering : NetworkBehaviour {

    public static readonly float[] TARGET_VELOCITY_STEPS = {-1, 0, 1, 5, 10};
    public enum Thruster {
        ROTATE_LEFT,
        ROTATE_RIGHT,
        TRANSLATE_LEFT,
        TRANSLATE_RIGHT,
        TRANSLATE_FORWARD,
        TRANSLATE_BACKWARD,
    }

    // z ignored, but using Vector3 for ez rotation~
    private NetworkVariable<Vector3> m_Velocity = new NetworkVariable<Vector3>();
    private NetworkVariable<int> m_TargetVelocityIdx = new NetworkVariable<int>(1);
    private NetworkVariable<float> m_AngularVelocity = new NetworkVariable<float>(0);
    private bool[] m_ThrusterStates;


    void Awake() {
        m_ThrusterStates = new bool[Enum.GetValues(typeof(Thruster)).Length];
    }

    public bool GetThrusterState(Thruster t) {
        return m_ThrusterStates[(int)t];
    }

    [ServerRpc(RequireOwnership=false)]
    public void SetThrusterStateServerRpc(Thruster t, bool state) {
        m_ThrusterStates[(int)t] = state;
    }
 
    [ServerRpc(RequireOwnership=false)]
    public void ChangeTargetVelocityServerRpc(bool up) {
        int delta_idx = up ? 1 : -1;
        m_TargetVelocityIdx.Value = Mathf.Clamp(m_TargetVelocityIdx.Value += delta_idx, 0, TARGET_VELOCITY_STEPS.Length - 1);
    }

    public float GetTargetSpeed() {
        return TARGET_VELOCITY_STEPS[m_TargetVelocityIdx.Value];
    }

    public float GetSpeed() {
        return Vector3.Magnitude((Vector2)m_Velocity.Value);
    }

    public static readonly float TRANSLATION_ACCELERATION = 0.1f;
    public static readonly float ROTATION_ACCELERATION = 0.15f;

    private void ApplyAllActiveThrusters(float delta_time) {
        string active = "";
        foreach (Thruster thruster in Enum.GetValues(typeof(Thruster))) {
            if (GetThrusterState(thruster)) {
                ApplyThruster(thruster, delta_time);
                active += thruster + " ";
            }
        }
        Debug.Log("Active Thrusters: " + active);
    }

    private void ApplyThruster(Thruster thruster, float delta_time) {
        switch(thruster) {
            case Thruster.TRANSLATE_FORWARD:
                m_Velocity.Value += new Vector3(1, 0, 0) * TRANSLATION_ACCELERATION * delta_time;
                break;
            case Thruster.TRANSLATE_BACKWARD:
                m_Velocity.Value -=  new Vector3(1, 0, 0) * TRANSLATION_ACCELERATION * delta_time;
                break;
            case Thruster.ROTATE_LEFT:
                m_AngularVelocity.Value += ROTATION_ACCELERATION * delta_time;
                break;
            case Thruster.ROTATE_RIGHT:
                m_AngularVelocity.Value -= ROTATION_ACCELERATION * delta_time;
                break;
            default:
                Debug.LogError("Thruster " + thruster + " not implemented yet");
                break;
        };
    }

    void Update() {
        // Set Thruster States
        Debug_MoveWithKeys(Time.deltaTime);
        if (IsServer) {
            UpdateServerside();
        }
    }

    // Server-side only
    void UpdateServerside() {
        float delta = Time.deltaTime;

        float target_x_velocity = TARGET_VELOCITY_STEPS[m_TargetVelocityIdx.Value];
        SetThrusterStateServerRpc(Thruster.TRANSLATE_FORWARD, m_Velocity.Value.x < target_x_velocity);
        SetThrusterStateServerRpc(Thruster.TRANSLATE_BACKWARD, m_Velocity.Value.x > target_x_velocity);

        // apply thrusters to set velocity
        ApplyAllActiveThrusters(delta);

        // rotate
        ShipManager.Instance.Rotate(m_AngularVelocity.Value);

        // translate
        Quaternion rot = Quaternion.AngleAxis(ShipManager.Instance.GetShipAngle(), Vector3.forward);
        ShipManager.Instance.Move(rot * m_Velocity.Value);
    }

    private void Debug_MoveWithKeys(float delta_time) {

        if (Input.GetKeyDown(KeyCode.I)) {
            ChangeTargetVelocityServerRpc(up: true);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            ChangeTargetVelocityServerRpc(up: false);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            SetThrusterStateServerRpc(Thruster.ROTATE_LEFT, true);
        }
        if (Input.GetKeyUp(KeyCode.J)) {
            SetThrusterStateServerRpc(Thruster.ROTATE_LEFT, false);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            SetThrusterStateServerRpc(Thruster.ROTATE_RIGHT, true);
        }
        if (Input.GetKeyUp(KeyCode.L)) {
            SetThrusterStateServerRpc(Thruster.ROTATE_RIGHT, false);
        }
    }
}
