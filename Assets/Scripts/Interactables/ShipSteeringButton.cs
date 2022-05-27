using UnityEngine;

public class ShipSteeringButton : SecondaryButton {
    public enum Action {
        NONE,
        TOGGLE_THRUSTER_TURN_LEFT,
        TOGGLE_THRUSTER_TURN_RIGHT,
        TARGET_SPEED_UP,
        TARGET_SPEED_DOWN,
    }

    [SerializeField]
    private Action m_Action;

    protected override void Interaction() {
        switch (m_Action) {
            case (Action.TOGGLE_THRUSTER_TURN_LEFT):
                ShipManager.Instance.Steering.ToggleThrusterStateServerRpc(ShipSteering.Thruster.ROTATE_LEFT);
                break;
            case (Action.TOGGLE_THRUSTER_TURN_RIGHT):
                ShipManager.Instance.Steering.ToggleThrusterStateServerRpc(ShipSteering.Thruster.ROTATE_RIGHT);
                break;
            case (Action.TARGET_SPEED_UP):
                ShipManager.Instance.Steering.ChangeTargetVelocityServerRpc(up: true);
                break;
            case (Action.TARGET_SPEED_DOWN):
                ShipManager.Instance.Steering.ChangeTargetVelocityServerRpc(up: false);
                break;
            default:
                break;
        }
    }
}
