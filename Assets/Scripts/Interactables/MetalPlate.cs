using UnityEngine;
using Unity.Netcode;

public class MetalPlate : DroppableInteractable {

    private NetworkVariable<bool> m_OnWall = new NetworkVariable<bool>(false);

    public override bool PlayerCanInteract() {
        return base.PlayerCanInteract() && !m_OnWall.Value;
    }

    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        interactionAudioSource.gameObject.transform.position = PlayerManager.Instance.LocalPlayer.Avatar.gameObject.transform.position;
        AudioClip sound = GetRandomInteractionClip();
        interactionAudioSource.PlayOneShot(sound);
        return PlayerAnimation.DRINK;
    }

    public override string FriendlyName() {
        return "Metal Plate";
    }

    [ServerRpc(RequireOwnership = false)]
    public void StickToWallServerRpc(Vector3 pos) {
        DropFromLocalPlayer();
        m_OnWall.Value = true;
        m_State.Value = IN_WORLD_NO_RIGIDBODY;
        transform.position = pos;
    }
}
