using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerReady : Interactable<bool> {
    PlayerAvatar player;
    private bool m_LocalPlayerInteracting = false;

    protected override void Interaction(){
        Debug.Log("interaction");
        /* if (!m_LocalPlayerInteracting && Value) {
            // cockpit occupied
            return;
        }
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(m_LocalPlayerInteracting);
        SetPlayerConditions(m_LocalPlayerInteracting); */
    }
    void SetPlayerConditions(bool on){
        GameObject status = GameObject.FindGameObjectWithTag("Status");
        if(on){
            status.GetComponent<TMP_Text>().text = "Ready";
            status.GetComponent<TMP_Text>().color = Color.green;
        }
        //PlayerManager.Instance.LocalPlayer.Avatar.notready.SetActive(!on);
    }
    public override void OnStateChange(bool previous, bool current){
        //SetPlayerConditions(current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

/*     private void Awake() {
        SetPlayerConditions(Value);
    } */


}
