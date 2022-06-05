using UnityEngine;
using Unity.Netcode;
using System.Collections;
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
    [SerializeField]
    private AudioClip[] gulpSounds;
    private AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player = GetComponent<PlayerAvatar>();
    }

    private AudioClip GetRandomClip(){
        return gulpSounds[UnityEngine.Random.Range(0, gulpSounds.Length)];
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

    IEnumerator WaitForMouth(float waitTime){
        yield return new WaitForSeconds(waitTime);
        AudioClip sound = GetRandomClip();
        audioSource.PlayOneShot(sound);
        Debug.Log("gulp");
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
                Debug.Log(m_Player.GetInventoryItem(PlayerAvatar.Slot.PRIMARY).name);
                if(m_Player.GetInventoryItem(PlayerAvatar.Slot.PRIMARY).tag == "Drink"){
                    StartCoroutine(WaitForMouth(0.5f));
                }
                
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                TriggerAnimation(PlayerAnimation.SIT);
            }
        }
    }
}
