using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayButton : InteractableBase {

    public string buttonName;
    [SerializeField]
    protected FotoboothDisplay display;
    protected override void Interaction(){
        if(buttonName == "Left"){
            display.SelectButtonLeft();
            display.Test();
            Debug.Log("Left Button");
        }
        if(buttonName == "Right"){
            display.SelectButtonRight();
            display.Test();
            Debug.Log("Right Button");
        }
        if(buttonName == "Confirm"){
            display.Select();
            display.Test();
            Debug.Log("Confirm Button");
        }
        if(buttonName == "Cancel"){
            display.Back();
            display.Test();
            Debug.Log("Cancel Button");
        }
    }
    public override string FriendlyName() {
        return buttonName;
    } 
}

/* 
private Material selectedMaterial;

selectedMaterial = PlayerManager.Instance.LocalPlayer.Avatar.GetComponent<Renderer>().material;
PlayerManager.Instance.LocalPlayer.Avatar.GetComponent<Renderer>().material = textureModels[index].GetComponent<Renderer>().material;
        selectedMaterial = PlayerManager.Instance.LocalPlayer.Avatar.GetComponent<Renderer>().material;
        Debug.Log(textureModels[index].GetComponent<Renderer>().material);

 */
