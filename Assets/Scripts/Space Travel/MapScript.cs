using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class MapScript : Interactable<bool> {

    public CinemachineVirtualCamera map_cam;
    protected override void Interaction(){
        SetServerRpc(!Value);
    }

    public override void OnStateChange(bool previous, bool current) {
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
        map_cam.Priority = 100;
        if(!value){
            map_cam.Priority = 0;
        }

    }
    private void Awake() {
    }
}
