using UnityEngine;

public class ShipSteeringButton : SecondaryButton {
    public enum Action {
        NONE,
        TOGGLE_THRUSTER_TURN_LEFT
    }

    [SerializeField]
    private Action m_Action;

    protected override void Interaction() {
        Debug.Log(m_Action + " pressed");
        switch (m_Action) {
            case (Action.TOGGLE_THRUSTER_TURN_LEFT):
                ShipManager.Instance.Steering.ToggleThrusterStateServerRpc(ShipSteering.Thruster.ROTATE_LEFT);
                break;
            default:
                break;
        }
    }
}
