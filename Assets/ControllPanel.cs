using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO remove class
public class ControllPanel : RangedInteractableBase
{
    [SerializeField]
    private float accaleration = 0.1f;
    public bool stearing;

    protected override void Interaction(){
        if(!stearing){
            // ShipManager.Instance.AccillerateSpeedServerRpc(accaleration);
        }
        else{
            // ShipManager.Instance.AccillerateAngleServerRpc(accaleration);
        }
    }
}
