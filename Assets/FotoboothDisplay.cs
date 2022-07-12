using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FotoboothDisplay : TwoLevelInteractable {
    [SerializeField]
    protected GameObject[] textureModels;
    private int index = 0;
    private Material selectedMaterial;



    public override void Start() {
        base.Start();
        /* index = PlayerPrefs.GetInt("CharacterSelected"); */

        selectedMaterial = PlayerManager.Instance.LocalPlayer.Avatar.GetComponent<Renderer>().material;

        foreach(GameObject go in textureModels){
            go.SetActive(false);
        }
        if(textureModels[index]){
            textureModels[index].SetActive(true);
        } 
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(index);

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
        /* PlayerPrefs.SetInt("CharacterSelected", index); */
        PlayerManager.Instance.LocalPlayer.Avatar.normalMaterial = textureModels[index].GetComponent<Renderer>().material;
        selectedMaterial = PlayerManager.Instance.LocalPlayer.Avatar.normalMaterial;
/*         var characters = GameObject.FindGameObjectsWithTag("CharacterList"); */
        Debug.Log(textureModels[index].GetComponent<Renderer>().material);
/*         for(int i = 0; i< characters[0].transform.childCount; i++){
            if(characterList[index].name == characters[0].transform.GetChild(i).name){
                PlayerManager.Instance.LocalPlayer.Avatar.SetActiveCharacter(i);
                PlayerManager.Instance.LocalPlayer.GetComponent<Animator>().avatar =; 
            }
            else characters[0].transform.GetChild(i).gameObject.SetActive(false);
        } */
    }
    public void Back(){
        Debug.Log("Cancel");
    }

    public void Test(){
        Debug.Log("working");
    }

    public override string FriendlyName() {
        return "Display";
    }
}
