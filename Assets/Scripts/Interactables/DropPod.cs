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

    public override string FriendlyName() {
        return "Delivery Airlock";
    }
}
