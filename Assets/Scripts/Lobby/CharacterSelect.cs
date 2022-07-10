using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {
    public List<Material> shirtColor;
    private int index;
    public GameObject canvas;
    public CinemachineVirtualCamera cam;


    void Start() {
        index = PlayerPrefs.GetInt("CharacterSelected");
    }

    public void SelectButton(bool right){/* 
        shirtColor[index].SetActive(false);
        if(right){
            index++;
            if(index == shirtColor.Length){
                index = 0;
            }
        } else {
            index--;
            if(index < 0){
                index = shirtColor.Length -1;  
            }
        }
        shirtColor[index].SetActive(true); */
        
    }

    public void Select(){
        PlayerPrefs.SetInt("CharacterSelected", index);
        var characters = GameObject.FindGameObjectsWithTag("shirtColor");
        Debug.Log(characters[0].name);
        for(int i = 0; i< characters[0].transform.childCount; i++){
            if(shirtColor[index].name == characters[0].transform.GetChild(i).name){
                PlayerManager.Instance.LocalPlayer.Avatar.SetActiveCharacter(i);
                /* PlayerManager.Instance.LocalPlayer.GetComponent<Animator>().avatar =; */
            }
            //else characters[0].transform.GetChild(i).gameObject.SetActive(false);
        }
        canvas.SetActive(false);
        cam.Priority = 0;
    }
    public void Back(){
        canvas.SetActive(false);
        cam.Priority = 0;
    }

}
