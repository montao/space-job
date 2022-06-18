using UnityEngine;
using Unity.Netcode;

public class RevivalFloppy : DroppableInteractable {
    public NetworkVariable<NetworkObjectReference> Player = new NetworkVariable<NetworkObjectReference>();

    // TODO move to medbay
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        Debug.Log("REVIVE!");
        return base.SelfInteraction(avatar);
    }
}
