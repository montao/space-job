using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerReadyButton : Interactable<bool> {
    PlayerAvatar player;
    private int readyCounter = 0;
    private bool m_LocalPlayerInteracting = false;

    protected override void Interaction(){
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(m_LocalPlayerInteracting);
        SetPlayerConditions(m_LocalPlayerInteracting); 
        SetReadyConditions(Value);

    }
    void SetPlayerConditions(bool on){
        PlayerManager.Instance.LocalPlayer.Avatar.ready.Value = on;   
        
        /* if(readyCounter == PlayerManager.Instance.Players.Count)  NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single); */

    }
    void SetReadyConditions(bool ready){
        foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
            if(IsOwner){
                if (PlayerManager.Instance.LocalPlayer.Avatar.ready.Value) {
                    readyCounter++;
               
                }
            }
            
        } 
        Debug.Log("ready counter: " + readyCounter + " Player Counter: " + PlayerManager.Instance.Players.Count);
    }

    void FixedUpdate() {
        if(readyCounter >= PlayerManager.Instance.Players.Count)  NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public override void OnStateChange(bool previous, bool current){
        //SetPlayerConditions(current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

}

