using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PipeWrench : DroppableInteractable{
    private bool m_PickedUp = false;  // Server-Only
    [SerializeField]
    private AudioClip[] swooshSounds;
    private AudioSource audioSource;

    public override void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();    
    }
    private AudioClip GetRandomSwooshClip(){
        return swooshSounds[UnityEngine.Random.Range(0, swooshSounds.Length)];
    }
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        AudioClip sound = GetRandomSwooshClip();
        audioSource.PlayOneShot(sound);
        return PlayerAnimation.INTERACT;
    }

    [ServerRpc(RequireOwnership = false)]
    public override void SetHolderServerRpc(int holder_id) {
        base.SetHolderServerRpc(holder_id);
        m_PickedUp = true;
        Debug.Log("Pick pipewrench");
    }

    public bool IsPickedUp() {
        return m_PickedUp;
    }
}
