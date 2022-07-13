using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Syringe : DroppableInteractable{
    private bool m_PickedUp = false;  // Server-Only
    [SerializeField]
    private AudioClip[] hurtSound;

    public override void Awake() {
        base.Awake();
    }

    public override string FriendlyName() {
        return "Wonder Heal";
    }

    private AudioClip GetRandomHurtClip(){
        return hurtSound[UnityEngine.Random.Range(0, hurtSound.Length)];
    }
    IEnumerator WaitForMouth(float waitTime){
        yield return new WaitForSeconds(waitTime);
        AudioClip sound = GetRandomHurtClip();
        audioSource.PlayOneShot(sound);
        //Debug.Log("gulp");
    }
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        avatar.HealBoost();
        StartCoroutine(WaitForMouth(0.5f));
        PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY).Despawn();
        return PlayerAnimation.SPRAY;
    }

    [ServerRpc(RequireOwnership = false)]
    public override void SetHolderServerRpc(int holder_id) {
        base.SetHolderServerRpc(holder_id);
        m_PickedUp = true;
    }

    public bool IsPickedUp() {
        return m_PickedUp;
    }
}
