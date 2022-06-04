using UnityEngine;
using Unity.Netcode;

public enum PlayerAnimation {
    ARMWAVE,
    SIT,
    INTERACT,
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

    void Update() {
        if (IsOwner) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                TriggerAnimationClientRpc(PlayerAnimation.ARMWAVE);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                TriggerAnimationClientRpc(PlayerAnimation.JUMP);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                TriggerAnimationClientRpc(PlayerAnimation.DRINK);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                TriggerAnimationClientRpc(PlayerAnimation.SIT);
            }
        }
    }
}
