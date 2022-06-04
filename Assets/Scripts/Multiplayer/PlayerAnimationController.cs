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
        Debug.Log("PlayerAnimationController: " + "hello" + m_PlayerAnimator + m_Player);
    }

    [ClientRpc]
    public void TriggerAnimationClientRpc(PlayerAnimation animation){
        Debug.Log("Triggering " + animation.ToString().ToLower());
        m_PlayerAnimator.SetTrigger(animation.ToString().ToLower());
    }

    public void OnSpeedChange(float prev, float current){
        Debug.Log("Speed: " + current);
        m_PlayerAnimator.SetFloat("speed", current);
    }
}
