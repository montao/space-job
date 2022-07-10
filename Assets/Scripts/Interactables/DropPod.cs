using UnityEngine;
using Unity.Netcode;

public class DropPod : RangedInteractableBase {
    private bool m_CanBeUsed = false;
    protected override void Interaction(){
        if (PlayerAvatar.IsHolding<MeetBalls>() && m_CanBeUsed) {
            FullFillOrderServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void FullFillOrderServerRpc(){
        ShipManager.Instance.MarkNearestDestinationAsReached();
        if (ShipManager.Instance.GetNearestDestination().pos.magnitude > 2.0f * (Map.MAX - Map.MIN)) {
            // all destinations reached
            ShipManager.Instance.SetWon();
        }
    }

    public override void Update() {
        base.Update();
        if(ShipManager.Instance.GetDistantToWin() <= ShipManager.WIN_DISTANCE_THRESHOLD){
            m_CanBeUsed = true;
        }
        else{
            m_CanBeUsed = false;
        }
    }

    public void SetState(bool new_state){
        m_CanBeUsed = new_state;
    }

    public override string FriendlyName() {
        return "CargoDropPod";
    }
}
