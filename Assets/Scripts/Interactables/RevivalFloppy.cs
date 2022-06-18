using UnityEngine;
using Unity.Netcode;

public class RevivalFloppy : DroppableInteractable {
    public NetworkVariable<NetworkObjectReference> Player = new NetworkVariable<NetworkObjectReference>();

    // TODO move to medbay
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar_reviving) {
        NetworkObject avatar_to_revive_neto;
        if (Player.Value.TryGet(out avatar_to_revive_neto)) {
            avatar_to_revive_neto.GetComponent<PlayerAvatar>().ReviveServerRpc(transform.position);
        } else {
            Debug.LogError("Attempted to revive, but no playeravatar object found.");
        }

        return base.SelfInteraction(avatar_reviving);  // TODO? animation?
    }
}
