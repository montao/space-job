using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class FotoboothDisplay : TwoLevelInteractable {
    [SerializeField]
    protected GameObject[] textureModels;
    private int index = 0;
    private Material selectedMaterial;
    private CameraSwap cam;




    public override void Start() {
        base.Start();
        /* index = PlayerPrefs.GetInt("CharacterSelected"); */
        cam = GetComponent<CameraSwap>();
        foreach(GameObject go in textureModels){
            go.SetActive(false);
        }
        if(textureModels[index]){
            textureModels[index].SetActive(true);
        } 
    }

    public void SelectButtonRight(){
        textureModels[index].SetActive(false);
        index++;
        if(index == textureModels.Length){
            index = 0;
        }
        textureModels[index].SetActive(true);
        
    }
    public void SelectButtonLeft(){
        textureModels[index].SetActive(false);
        index--;
        if(index < 0){
            index = textureModels.Length -1;  
        }
        textureModels[index].SetActive(true);
        
    }
    public void Select(){
        PlayerManager.Instance.LocalPlayer.Avatar.SetTexture(index);
        cam.SwitchAway();
        

    }
    public void Back(){
        PlayerManager.Instance.LocalPlayer.Avatar.SetTexture(0);
        index = 0;
        cam.SwitchAway();
        Debug.Log("Cancel");
    }

    public void Test(){
        Debug.Log("working");
    }

    public override string FriendlyName() {
        return "Display";
    }
}
