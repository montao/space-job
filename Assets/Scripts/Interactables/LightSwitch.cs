using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LightSwitch : Interactable<bool> {
    public List<Light> Lights;

    protected override void Interaction(){
        SetLightConditions(!Value);
        SetServerRpc(!Value);
    }
    public override void OnStateChange(bool previous, bool current){
        SetLightConditions(current);
    }

    void SetLightConditions(bool on){
        foreach(var light in Lights){
            light.gameObject.SetActive(on);
        }
    }
    private void Awake() {
        SetLightConditions(Value);
    }
}
