using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Energy : DroppableInteractable{
    public bool Strawberry;
    private bool m_PickedUp = false;  // Server-Only
    [SerializeField]
    private AudioClip[] gulpSounds;
    private int m_Usages = 0;

    public override void Awake() {
        base.Awake();
    }

    public override string FriendlyName() {
        if(Strawberry){
            return "Late Night Strawberry";
        }
        return "Tangerine Maschine";
    }

    private AudioClip GetRandomGulpClip(){
        return gulpSounds[UnityEngine.Random.Range(0, gulpSounds.Length)];
    }
    IEnumerator WaitForMouth(float waitTime){
        yield return new WaitForSeconds(waitTime);
        AudioClip sound = GetRandomGulpClip();
        audioSource.PlayOneShot(sound);
        Debug.Log("gulp");
    }
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        avatar.SpeedBoost();
        StartCoroutine(WaitForMouth(0.5f));
        if(m_Usages == 3){
            PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY).Despawn();
        }
        m_Usages ++;
        return PlayerAnimation.DRINK;
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
