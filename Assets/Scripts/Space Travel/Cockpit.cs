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
        SetCockConditions(!Value);
    }

    void SetCockConditions(bool on){
        cam.Priority = cam_prio;
        foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
            Debug.Log(player.Avatar.name);
            player.Avatar.GetComponent<CharacterController>().enabled = on;
        }
        stars.GetComponent<Movnt>().enabled = on;
        if(on){
            cam.Priority = 0;
        }
    }
    public override void OnStateChange(bool previous, bool current){
        SetCockConditions(previous);
    }
    [ServerRpc(RequireOwnership = false)]

    public void SetServerRpc(bool value){
        m_State.Value = value;
        
        
    }

}
