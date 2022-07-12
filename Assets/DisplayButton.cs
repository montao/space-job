using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayButton : InteractableBase {
    
    public string buttonName;
    [SerializeField]
    protected FotoboothDisplay display;
    protected override void Interaction(){
        if(buttonName == "Left"){
            Debug.Log("Left Button");
        }
        if(buttonName == "Right"){
            Debug.Log("Right Button");
        }
        if(buttonName == "Confirm"){
            Debug.Log("Confirm Button");
        }
        if(buttonName == "Cancel"){
            Debug.Log("Cancel Button");
        }
    }
    public override string FriendlyName() {
        return buttonName;
    } 
}
