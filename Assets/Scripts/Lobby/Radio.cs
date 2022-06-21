using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class Radio : Interactable<bool> {

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        SetRadioConditions(Value); 
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }
    protected override void Interaction(){
        SetRadioConditions(!Value);
        SetServerRpc(!Value);
        
    }
    
    public override void OnStateChange(bool previous, bool current){
        SetRadioConditions(current);
    }

    void SetRadioConditions(bool on){
        if(on){
            
            audioSource.UnPause();
            Debug.Log("Music playing");
        }
        if(!on){
            audioSource.Pause();
            Debug.Log("Music stopped playing");
        }
        
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer){
            m_State.Value = true;
        }
    }
    
}
