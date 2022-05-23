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
        if (!m_LocalPlayerInteracting && Value) {
            // cockpit occupied
            return;
        } 
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(/* Value */m_LocalPlayerInteracting );
        SetPlayerConditions(/* Value */ m_LocalPlayerInteracting); 
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

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

   /*  private void Awake() {
        SetPlayerConditions(Value);
    }  */


}
