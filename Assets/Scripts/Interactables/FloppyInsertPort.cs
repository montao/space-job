using UnityEngine;
using Unity.Netcode;


public class FloppyInsertPort : InteractableBase {
    public NetworkVariable<NetworkObjectReference> PlayerData = new NetworkVariable<NetworkObjectReference>();

    protected override void Interaction(){
        PlayerAvatar activePlayer = PlayerManager.Instance.LocalPlayer.Avatar;
        if (PlayerAvatar.IsHolding<RevivalFloppy>()) {
            DroppableInteractable item = Util.GetDroppableInteractable(activePlayer.GetInventoryItem(PlayerAvatar.Slot.PRIMARY));
            RevivalFloppy floppy = item.GetComponent<RevivalFloppy>();
            SetPlayerReferenceServerRpc(floppy.Player.Value);
            activePlayer.DropItem(PlayerAvatar.Slot.PRIMARY);
            floppy.DespawnServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReferenceServerRpc(NetworkObjectReference reference) {
        var player = Util.TryGet<PlayerAvatar>(reference);
        Debug.Log("Floppy Inserted to revive " + (player != null ? player.name : "null") + "!");
        PlayerData.Value = reference;
    }
}
