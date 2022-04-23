using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoffeMachine : Interactable<bool>
{
    public GameObject Cup;
    public override void OnStateChange(bool previous, bool current)
    {
        throw new System.NotImplementedException();
    }
    protected override void Interaction(){
        SetServerRpc(!Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
        GameObject freshCup = Instantiate(Cup, Vector3.zero, Quaternion.identity);
        freshCup.GetComponent<NetworkObject>().Spawn();
    }
    private void Awake() {
    }
}
