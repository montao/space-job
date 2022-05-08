using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

[RequireComponent(typeof(NetworkObject))]
public class PlayerReady : NetworkBehaviour {
    private PlayerAvatar player;
    public bool ready = false;
    public bool notReady = false;

    void Start() {
        player = PlayerManager.Instance.LocalPlayer.Avatar;
        notReady = true;
        
    }


    public void Ready(){
        ready = true;
        notReady = false;
    }

    void Update(){
        if(notReady){
            player.notReady.SetActive(true);
            Debug.Log("not ready");
        }
        if(ready){
            player.notReady.SetActive(false);
            player.ready.SetActive(true);
            Debug.Log("ready"); 
        }
    }
}
