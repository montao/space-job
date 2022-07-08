using UnityEngine;

public class MechanicalSpeedLever : TwoLevelInteractable {

    [SerializeField]
    private Transform m_SpinningThingy;
    public static readonly float[] VELOCITY_STEPS_ANGLES = {-30f, 30f, 90f, 150f, 210f};
    public static float ANIMATION_SPEED = 140.0f;  // degrees/sec (i think)

    private float rot_z;

    public override void Awake() {
        base.Awake();
        rot_z = m_SpinningThingy.rotation.eulerAngles.z;
    }

    public override void Update() {
        base.Update();
        var speed = ShipManager.Instance.Steering.GetTargetSpeedIndex();
        var angle = VELOCITY_STEPS_ANGLES[speed];

        if (Mathf.Abs(rot_z - angle) < 1f) {
            rot_z = angle;
        }
        if (rot_z < angle) {
            rot_z += ANIMATION_SPEED * Time.deltaTime;
            if (rot_z > angle) {  // no over-correction!
                rot_z = angle;
            }
        } else if (rot_z > angle) {
            rot_z -= ANIMATION_SPEED * Time.deltaTime;
            if (rot_z < angle) {
                rot_z = angle;
            }
        }
        rot_z = Mathf.Clamp(rot_z, -30f, 210f);
        if (Mathf.Abs(rot_z - angle) < 1f) {
            rot_z = angle;
        }

        var rot = m_SpinningThingy.eulerAngles;
        rot.z = rot_z;
        m_SpinningThingy.eulerAngles = rot;
    }
}
