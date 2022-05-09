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

    void PlayerStatus() {
        if(notReady){

            player.ChangeStatus("Not Ready", Color.red);
            Debug.Log("not ready");
        }
        if(!notReady){
            player.ChangeStatus("Ready", Color.green);
            Debug.Log("ready"); 
        }
    }


    public void Ready(){
        notReady = false;
    }

    void Update(){
        foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
            if (player.OwnerClientId == OwnerClientId) {
                PlayerStatus();
                break;
            }
        }
        
    }
}
