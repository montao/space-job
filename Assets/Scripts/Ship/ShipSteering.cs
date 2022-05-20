using UnityEngine;
using System;

[RequireComponent(typeof(ShipManager))]
public class ShipSteering : MonoBehaviour {

    private Vector3 m_Velocity;  // z ignored, but using Vector3 for ez rotation~
    private float[] m_TargetVelocityForwardSteps = {-1, 0, 1, 5, 10};
    private int m_TargetVelocityIdx = 1;
    private float m_AngularVelocity;

    public enum Thruster {
        ROTATE_LEFT,
        ROTATE_RIGHT,
        TRANSLATE_LEFT,
        TRANSLATE_RIGHT,
        TRANSLATE_FORWARD,
        TRANSLATE_BACKWARD,
    }
    private bool[] m_ThrusterStates;

    void Awake() {
        m_ThrusterStates = new bool[Enum.GetValues(typeof(Thruster)).Length];
    }

    public bool GetThrusterState(Thruster t) {
        return m_ThrusterStates[(int)t];
    }
    public void SetThrusterState(Thruster t, bool state) {
        m_ThrusterStates[(int)t] = state;
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
                m_Velocity += new Vector3(1, 0, 0) * TRANSLATION_ACCELERATION * delta_time;
                break;
            case Thruster.TRANSLATE_BACKWARD:
                m_Velocity -=  new Vector3(1, 0, 0) * TRANSLATION_ACCELERATION * delta_time;
                break;
            case Thruster.ROTATE_LEFT:
                m_AngularVelocity += ROTATION_ACCELERATION * delta_time;
                break;
            case Thruster.ROTATE_RIGHT:
                m_AngularVelocity -= ROTATION_ACCELERATION * delta_time;
                break;
            default:
                Debug.LogError("Thruster " + thruster + " not implemented yet");
                break;
        };
    }

    void Update() {
        float delta = Time.deltaTime;

        // Set Thruster States
        Debug_MoveWithKeys(delta);

        float target_x_velocity = m_TargetVelocityForwardSteps[m_TargetVelocityIdx];
        SetThrusterState(Thruster.TRANSLATE_FORWARD, m_Velocity.x < target_x_velocity);
        SetThrusterState(Thruster.TRANSLATE_BACKWARD, m_Velocity.x > target_x_velocity);

        // apply thrusters to set velocity
        ApplyAllActiveThrusters(delta);

        // rotate
        ShipManager.Instance.Rotate(m_AngularVelocity);

        // translate
        Quaternion rot = Quaternion.AngleAxis(ShipManager.Instance.GetShipAngle(), Vector3.forward);
        ShipManager.Instance.Move(rot * m_Velocity);
    }

    public void ChangeTargetVelocity(bool up) {
        int delta_idx = up ? 1 : -1;
        m_TargetVelocityIdx = Mathf.Clamp(m_TargetVelocityIdx += delta_idx, 0, m_TargetVelocityForwardSteps.Length - 1);
    }
    public float GetTargetSpeed() {
        return m_TargetVelocityForwardSteps[m_TargetVelocityIdx];
    }
    public float GetSpeed() {
        return Vector3.Magnitude((Vector2)m_Velocity);
    }

    private void Debug_MoveWithKeys(float delta_time) {

        if (Input.GetKeyDown(KeyCode.I)) {
            ChangeTargetVelocity(up: true);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            ChangeTargetVelocity(up: false);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            SetThrusterState(Thruster.ROTATE_LEFT, true);
        }
        if (Input.GetKeyUp(KeyCode.J)) {
            SetThrusterState(Thruster.ROTATE_LEFT, false);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            SetThrusterState(Thruster.ROTATE_RIGHT, true);
        }
        if (Input.GetKeyUp(KeyCode.L)) {
            SetThrusterState(Thruster.ROTATE_RIGHT, false);
        }
    }
}
