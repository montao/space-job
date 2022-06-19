using UnityEngine;
using Unity.Netcode;


public class FloppyInsertPort : InteractableBase {
    private NetworkVariable<NetworkObjectReference> m_PlayerData = new NetworkVariable<NetworkObjectReference>();

    protected override void Interaction(){
        PlayerAvatar activePlayer = PlayerManager.Instance.LocalPlayer.Avatar;
        if (PlayerAvatar.IsHolding<RevivalFloppy>()) {
            DroppableInteractable item = Util.GetDroppableInteractable(activePlayer.GetInventoryItem(PlayerAvatar.Slot.PRIMARY));
            RevivalFloppy floppy = item.GetComponent<RevivalFloppy>();
            SetPlayerReferenceServerRpc(floppy.Player.Value);
            activePlayer.DropItem(PlayerAvatar.Slot.PRIMARY);
            floppy.DespawnServerRpc();
            Debug.Log("Cromch");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReferenceServerRpc(NetworkObjectReference reference) {
        Debug.Log("Floppy Inserted!");
        m_PlayerData.Value = reference;
    }
}
