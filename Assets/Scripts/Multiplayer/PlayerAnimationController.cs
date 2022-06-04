using UnityEngine;
using Unity.Netcode;
using System;

public enum PlayerAnimation {
    NONE,
    ARMWAVE,
    INTERACT,
    SIT,
    JUMP,
    DRINK,
}

public class PlayerAnimationController : NetworkBehaviour {
    private Animator m_PlayerAnimator;
    private PlayerAvatar m_Player;

    private void Start() {
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player = GetComponent<PlayerAvatar>();
    }

    private void TriggerAnimationLocally(PlayerAnimation animation) {
        Debug.Log("Triggering " + animation.ToString().ToLower());
        foreach(PlayerAnimation ani in Enum.GetValues(typeof(PlayerAnimation))){
            if(ani != PlayerAnimation.NONE){
                m_PlayerAnimator.ResetTrigger(ani.ToString().ToLower());
            }
        }
        m_PlayerAnimator.SetTrigger(animation.ToString().ToLower());
    }

    public void TriggerAnimation(PlayerAnimation animation) {
        if (!IsOwner) {
            Debug.LogWarning("Only owner should call TriggerAnimation!");
            return;
        }
        TriggerAnimationLocally(animation);
        TriggerAnimationServerRpc(animation);
    }

    [ClientRpc]
    private void TriggerAnimationClientRpc(PlayerAnimation animation) {
        if (IsOwner) {
            return;  // Animation already triggered locally
        }
        if (animation == PlayerAnimation.NONE) {
            return;
        }

        TriggerAnimationLocally(animation);
    }

    [ServerRpc]
    private void TriggerAnimationServerRpc(PlayerAnimation animation) {
        TriggerAnimationClientRpc(animation);
    }

    public void OnSpeedChange(float speed){
        m_PlayerAnimator.SetFloat("speed", speed);
    }

    void Update() {
        if (IsOwner) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                TriggerAnimation(PlayerAnimation.ARMWAVE);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                TriggerAnimation(PlayerAnimation.JUMP);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                TriggerAnimation(PlayerAnimation.DRINK);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                TriggerAnimation(PlayerAnimation.SIT);
            }
        }
    }
}
