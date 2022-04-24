using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoffeMachine : Interactable<bool> {
    public GameObject cupPrefab;
    public GameObject machine;
   
    protected override void Interaction(){
        GameObject freshCup = Instantiate(cupPrefab, machine.GetComponent<Transform>().position, Quaternion.identity);
        freshCup.GetComponent<NetworkObject>().Spawn();
        Debug.Log("new cup");
        SetServerRpc(!Value);
    }

    public override void OnStateChange(bool previous, bool current) {
        SetServerRpc(current);
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }
    private void Awake() {
    }
}
