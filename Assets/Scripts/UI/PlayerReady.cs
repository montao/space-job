using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

[RequireComponent(typeof(NetworkObject))]
public class PlayerReady : NetworkBehaviour {
    private PlayerAvatar player;
    public bool notReady = false;

    void Start() {
        player = PlayerManager.Instance.LocalPlayer.Avatar;
        
        notReady = true;
        
    }

    public void PlayerStatusChanged() {
        if(notReady){

            player.statusText.text = "Not Ready";
            Debug.Log("not ready");
        }
        if(!notReady){
            player.statusText.color = Color.green;
            player.statusText.text = "Ready";
            Debug.Log("ready"); 
        }
    }


    public void Ready(){
        notReady = false;
    }

    void Update(){
        PlayerStatusChanged();
    }
}
