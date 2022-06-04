using UnityEngine;
using Unity.Netcode;

public enum PlayerAnimation {
    INTERACT,
    ARMWAVE,
    SIT,
    JUMP,
    DRINK,
}

public class PlayerAnimationController : NetworkBehaviour {
    private Animator m_PlayerAnimator;
    private PlayerAvatar m_Player;

    private void Awake() {
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player = GetComponent<PlayerAvatar>();
    }

    [ClientRpc]
    public void TriggerAnimationClientRpc(PlayerAnimation animation){
        Debug.Log("Triggering " + animation.ToString().ToLower());
        m_PlayerAnimator.SetTrigger(animation.ToString().ToLower());
    }

    public void OnSpeedChange(float speed){
        m_PlayerAnimator.SetFloat("speed", speed);
    }
}
