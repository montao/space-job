using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PipeWrench : DroppableInteractable{

/*     [SerializeField]
    private AudioClip[] swooshSounds;
    private AudioSource audioSource;

    public override void Awake() {
        audioSource = GetComponent<AudioSource>();    
    }
    private AudioClip GetRandomSwooshClip(){
        return swooshSounds[UnityEngine.Random.Range(0, swooshSounds.Length)];
    } */
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
/*         AudioClip sound = GetRandomSwooshClip();
        audioSource.PlayOneShot(sound); */
        return PlayerAnimation.INTERACT;
    }
}
