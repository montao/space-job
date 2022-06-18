using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerReadyButton : Interactable<bool> {

    private bool m_LocalPlayerInteracting = false;
    //int readyCouter = 0;
    public NetworkVariable<int> PlayersReady =
        new NetworkVariable<int>(0);    
        
    [ServerRpc(RequireOwnership = false)]
    public void ModReadyServerRpc(int i){
        PlayersReady.Value = PlayersReady.Value + i;
    }

    protected override void Interaction(){
        Debug.Log("ready" + PlayersReady.Value);
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(m_LocalPlayerInteracting);
        SetPlayerConditions(m_LocalPlayerInteracting);
        ModReadyServerRpc(m_LocalPlayerInteracting ? 1 : -1);
        Debug.Log("ready" + PlayersReady.Value);
    }
    void SetPlayerConditions(bool on){
        PlayerManager.Instance.LocalPlayer.Avatar.ready.Value = !PlayerManager.Instance.LocalPlayer.Avatar.ready.Value;  
    }

    public override void Update() {
        base.Update();
        if(PlayersReady.Value == PlayerManager.Instance.Players.Count){
            if(IsServer) {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= PlayerManager.Instance.StartShip;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.Instance.MovePlayersToSpawns;
                NetworkManager.Singleton.SceneManager.LoadScene("ShipScene", LoadSceneMode.Single);
            }
        }
    }
    public override void OnStateChange(bool previous, bool current){
        //SetPlayerConditions(current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

}

