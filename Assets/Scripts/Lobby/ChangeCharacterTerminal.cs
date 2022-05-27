using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeCharacterTerminal : Interactable<bool> {
    PlayerAvatar player;
    private bool m_LocalPlayerInteracting = false;
    public GameObject canvas;
    public CinemachineVirtualCamera cam;

    protected override void Interaction(){
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(m_LocalPlayerInteracting);
        SetPlayerConditions(m_LocalPlayerInteracting); 
    }
    void SetPlayerConditions(bool on){
        cam.Priority = 100;
        canvas.SetActive(true);
    }

    public override void OnStateChange(bool previous, bool current){
        //SetPlayerConditions(current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }


}
