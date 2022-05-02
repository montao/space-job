using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Interactable<bool>{
    public override void OnStateChange(bool previous, bool current){}
    protected override void Interaction(){}
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer){
            m_State.Value = true;
        }
    }
}
