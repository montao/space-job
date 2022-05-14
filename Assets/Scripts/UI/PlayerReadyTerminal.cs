using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using TMPro;

public class PlayerReadyTerminal : Interactable<bool> {
    PlayerAvatar player;
    private bool m_LocalPlayerInteracting = false;
    public GameObject canvas;
    public CinemachineVirtualCamera cam;

    protected override void Interaction(){
        SetPlayerConditions(!Value); 
    }
   [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }
    void SetPlayerConditions(bool on){
        PlayerManager.Instance.LocalPlayer.Avatar.ready.Value = on;
        cam.Priority = 100;
        canvas.SetActive(true);

        //PlayerManager.Instance.LocalPlayer.Avatar.notready.SetActive(!on);
    }
    public override void OnStateChange(bool previous, bool current){
        //SetPlayerConditions(current);
    }



   /*  private void Awake() {
        SetPlayerConditions(Value);
    }  */


}
