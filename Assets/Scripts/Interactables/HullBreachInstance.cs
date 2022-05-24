using UnityEngine;
using Unity.Netcode;

public class HullBreachInstance : RangedInteractableBase {

    private Room m_Room;
    private Transform m_SpawnLocation;

    protected override void Interaction() {
        if (PlayerAvatar.IsHolding<BreachBeGone>()) {
            ResolvedServerRpc();
        }
    }

    // called by room
    public void Setup(Room room, Transform spawn) {
        m_Room = room;
        m_SpawnLocation = spawn;
    }

    [ServerRpc(RequireOwnership=false)]
    public void ResolvedServerRpc() {
        Debug.Log("Hull Breach fixed in " + m_Room);
        m_Room.HullBreachResolved(this, m_SpawnLocation);
        GetComponent<NetworkObject>().Despawn(destroy: true);  // breach do be gone
    }
}
