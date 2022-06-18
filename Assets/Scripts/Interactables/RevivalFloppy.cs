using UnityEngine;
using Unity.Netcode;

public class RevivalFloppy : DroppableInteractable {
    public NetworkVariable<NetworkObjectReference> Player = new NetworkVariable<NetworkObjectReference>();

    // TODO move to medbay
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar_reviving) {
        Debug.Log("Revival Time!");
        NetworkObject avatar_to_revive_neto;
        CanInteract = false;
        var pos = avatar_reviving.transform.position + (1.5f * avatar_reviving.transform.forward) + (0.2f * Vector3.up);
        if (Player.Value.TryGet(out avatar_to_revive_neto)) {
            avatar_to_revive_neto.GetComponent<PlayerAvatar>().ReviveServerRpc(pos);
        } else {
            Debug.LogError("Attempted to revive, but no playeravatar object found.");
        }

        DespawnServerRpc();
        return base.SelfInteraction(avatar_reviving);  // TODO? animation?
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc() {
        GetComponentInParent<NetworkObject>().Despawn(destroy: true);
    }
}
