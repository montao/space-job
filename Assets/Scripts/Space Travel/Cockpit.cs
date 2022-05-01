using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class Cockpit : Interactable<bool> {

    public CinemachineVirtualCamera cam;
    public int cam_prio;
    public GameObject stars;
    

    protected override void Interaction(){
        SetServerRpc(!Value);
    }

    public override void OnStateChange(bool previous, bool current){
    }
    [ServerRpc(RequireOwnership = false)]

    public void SetServerRpc(bool value){
        m_State.Value = value;
        cam.Priority = cam_prio;
        foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
            Debug.Log(player.Avatar.name);
            player.Avatar.GetComponent<CharacterController>().enabled = false;
        }
        stars.GetComponent<Movnt>().enabled = true;
        
        
    }
    /* void Update(){
        if(Input.GetKeyDown(KeyCode.E)){
            cam_prio = 0;
            foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
                Debug.Log(player.Avatar.name);
                player.Avatar.GetComponent<CharacterController>().enabled = true;
            }
            stars.GetComponent<Movnt>().enabled = false;
        }
    } */
    private void Awake() {

    }

}
