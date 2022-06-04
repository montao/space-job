using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudio : MonoBehaviour {
    
    [SerializeField]
    private AudioClip[] stepSounds;
    public AudioClip take;
    public AudioClip clap;
    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();    
    }

    private void Step(){
        AudioClip sound = GetRandomClip();
        audioSource.PlayOneShot(sound);
        Debug.Log("step");
    }

    private void Take(){
        AudioClip sound = take;
        audioSource.PlayOneShot(take);
    }

    private void Clap(){
        AudioClip sound = clap;
        audioSource.PlayOneShot(clap);
    }
    private AudioClip GetRandomClip(){
        return stepSounds[UnityEngine.Random.Range(0, stepSounds.Length)];
    }

    private void NoHologram(){
        if(tag != "Hologram"){
            Step();
            Take();
            Clap();
        }
    }


}
