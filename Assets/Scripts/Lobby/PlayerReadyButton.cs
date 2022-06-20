using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerReadyButton : Interactable<bool> {

    private bool m_LocalPlayerInteracting = false;
    private static float countdownTime = 5.9f;
    private float timeRemaining = countdownTime;
    private bool countdownInAction = false;
    //int readyCouter = 0;
    public NetworkVariable<int> PlayersReady =
        new NetworkVariable<int>(0);
    public NetworkVariable<int> TimeShown =
        new NetworkVariable<int>(0);

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private TMP_Text secs;


    [ServerRpc(RequireOwnership = false)]
    public void ModReadyServerRpc(int i){
        PlayersReady.Value = PlayersReady.Value + i;
    }

    protected override void Interaction(){
        Debug.Log("ready" + PlayersReady.Value);
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(m_LocalPlayerInteracting);
        SetPlayerConditions(m_LocalPlayerInteracting);
        ModReadyServerRpc(PlayerManager.Instance.LocalPlayer.Avatar.ready.Value ? 1 : -1);
        Debug.Log("ready" + PlayersReady.Value);
    }
    void SetPlayerConditions(bool on){
        PlayerManager.Instance.LocalPlayer.Avatar.ready.Value = !PlayerManager.Instance.LocalPlayer.Avatar.ready.Value;  
    }

    public override void Update() {
        base.Update();
        if (PlayersReady.Value == PlayerManager.Instance.ConnectedPlayerCount){
            if (IsServer) {
                if (! countdownInAction) {
                    countdownInAction = true;
                }   
            }
            Countdown(true);
            canvas.gameObject.SetActive(true);
        }
        else {
            if (IsServer){
                countdownInAction = false;                
            }
            Countdown(false);
            canvas.gameObject.SetActive(false);
        }
    }
    public override void OnStateChange(bool previous, bool current){
        //SetPlayerConditions(current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

    public void Countdown(bool isrunning) {
        if (isrunning) {
            if (timeRemaining < 0) {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= PlayerManager.Instance.StartShip;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.Instance.MovePlayersToSpawns;
                NetworkManager.Singleton.SceneManager.LoadScene("ShipScene", LoadSceneMode.Single);
            }
            else {
                timeRemaining -= Time.deltaTime;
                TimeShown.Value = (Mathf.FloorToInt(timeRemaining % 60));
            }
        }
        else {
            timeRemaining = countdownTime;
        }
    }

    void OnGUI() {
        if(countdownInAction) {
            if (TimeShown.Value < 0) {
                secs.text = "loading...";
            }
            else {
                secs.text = TimeShown.Value.ToString();
            }  
        }
    }
}

