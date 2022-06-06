using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudio : MonoBehaviour {
    
    [SerializeField]
    private AudioClip[] stepSounds;
    [SerializeField]
    private AudioClip[] clapSounds;
    public AudioClip take;
    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();    
    }

    private void Step(){
        AudioClip sound = GetRandomStepClip();
        audioSource.PlayOneShot(sound);
    }

    private void Take(){
        AudioClip sound = take;
        audioSource.PlayOneShot(take);
    }

    private void Clap(){
        AudioClip sound = GetRandomClapClip();
        audioSource.PlayOneShot(sound);
    }
    private AudioClip GetRandomStepClip(){
        return stepSounds[UnityEngine.Random.Range(0, stepSounds.Length)];
    }
    private AudioClip GetRandomClapClip(){
        return clapSounds[UnityEngine.Random.Range(0, clapSounds.Length)];
    }

    private void NoHologram(){
        if(tag != "Hologram"){
            Step();
            Take();
            Clap();
        }
    }


}
