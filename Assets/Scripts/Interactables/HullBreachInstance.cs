using UnityEngine;
using Unity.Netcode;

public class HullBreachInstance : RangedInteractableBase {

    private Room m_Room;

    protected override void Interaction() {
        if (PlayerAvatar.IsHolding<BreachBeGone>()) {
            ResolvedServerRpc();
        }
    }

    // called by room
    public void Setup(Room room) {
        m_Room = room;
    }

    [ServerRpc(RequireOwnership=false)]
    public void ResolvedServerRpc() {
        Debug.Log("Hull Breach fixed in " + m_Room);
        m_Room.HullBreachResolved(this);
        GetComponent<NetworkObject>().Despawn(destroy: true);  // breach do be gone
    }
}
