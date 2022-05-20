using UnityEngine;

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

    public static readonly float TRANSLATION_ACCELERATION = 0.1f;
    public static readonly float ROTATION_ACCELERATION = 0.15f;

    public void FireThruster(Thruster thruster, float delta_time) {
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
                break;
        };
    }

    void Update() {
        float delta = Time.deltaTime;

        // Thrusters set up velocities
        Debug_MoveWithKeys(delta);

        // go towards set speed
        float target_x_velocity = m_TargetVelocityForwardSteps[m_TargetVelocityIdx];
        if (m_Velocity.x < target_x_velocity) {
            FireThruster(Thruster.TRANSLATE_FORWARD, delta);
        }
        if (m_Velocity.x > target_x_velocity) {
            FireThruster(Thruster.TRANSLATE_BACKWARD, delta);
        }

        // rotate
        ShipManager.Instance.Rotate(m_AngularVelocity);

        // translate
        Quaternion rot = Quaternion.AngleAxis(ShipManager.Instance.GetShipAngle(), Vector3.forward);
        ShipManager.Instance.Move(rot * m_Velocity);
    }

    public void ChangeTargetVelocity(bool up) {
        int delta = up ? 1 : -1;
        m_TargetVelocityIdx = Mathf.Clamp(m_TargetVelocityIdx += delta, 0, m_TargetVelocityForwardSteps.Length - 1);

        Debug.Log("Target fwd velocity: " + m_TargetVelocityForwardSteps[m_TargetVelocityIdx]);
    }

    private void Debug_MoveWithKeys(float delta_time) {
        if (Input.GetKeyDown(KeyCode.I)) {
            ChangeTargetVelocity(up: true);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            ChangeTargetVelocity(up: false);
        }
        if (Input.GetKey(KeyCode.J)) {
            FireThruster(Thruster.ROTATE_LEFT, delta_time);
        }
        if (Input.GetKey(KeyCode.L)) {
            FireThruster(Thruster.ROTATE_RIGHT, delta_time);
        }
    }
}
