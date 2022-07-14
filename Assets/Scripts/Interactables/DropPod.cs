using UnityEngine;
using Unity.Netcode;

public class DropPod : RangedInteractableBase {
    public bool m_CanBeUsed = false;
    private Animator doorAnimator;
    public override void Start(){
        doorAnimator = GetComponent<Animator>();
        base.Start();
    } 
    protected override void Interaction(){
        if (PlayerAvatar.IsHolding<MeetBalls>() && m_CanBeUsed) {
            FullFillOrderServerRpc();
            PlayerAvatar ava = PlayerManager.Instance.LocalPlayer.Avatar;
            if (ava) {
                Util.GetDroppableInteractable(ava.GetInventoryItem(PlayerAvatar.Slot.PRIMARY)).DespawnServerRpc();
            }
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
        //Debug.Log("m_CanBeUsed is: " + m_CanBeUsed);
        doorAnimator.SetBool("Open", m_CanBeUsed);
    }

    public override string FriendlyName() {
        return "Delivery Airlock";
    }
}
