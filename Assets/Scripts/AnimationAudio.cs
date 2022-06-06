using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationAudio : MonoBehaviour {
    
    [SerializeField]
    private AudioClip[] stepSounds;
    [SerializeField]
    private AudioClip[] stepCarpetSounds;
    [SerializeField]
    private AudioClip[] clapSounds;
    public AudioClip take;
    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();    
    }

    private void Step(){
        AudioClip sound = null;
        if(SceneManager.GetActiveScene().name == "Lobby"){
            sound = GetRandomStepClip();
            Debug.Log("Lobby Steps");
        }  else sound = GetRandomStepClip();
        audioSource.PlayOneShot(sound);
    }

    private void Take(){
        audioSource.PlayOneShot(take);
    }

    private void Clap(){
        AudioClip sound = GetRandomClapClip();
        audioSource.PlayOneShot(sound);
    }
    private AudioClip GetRandomStepClip(){
        return stepSounds[UnityEngine.Random.Range(0, stepSounds.Length)];
    }
    private AudioClip GetRandomCarpetStepClip(){
        return stepCarpetSounds[UnityEngine.Random.Range(0, stepCarpetSounds.Length)];
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
