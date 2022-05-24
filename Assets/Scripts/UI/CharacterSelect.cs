using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSelect : MonoBehaviour {
    private GameObject[] characterList;
    private int index;
    public GameObject canvas;
    public CinemachineVirtualCamera cam;

    void Start() {
        index = PlayerPrefs.GetInt("CharacterSelected");
        characterList = new GameObject[transform.childCount];
        for(int i = 0; i< transform.childCount; i++){
            characterList[i] = transform.GetChild(i).gameObject;
        }

        foreach(GameObject go in characterList){
            go.SetActive(false);
        }
        if(characterList[index]){
            characterList[index].SetActive(true);
        }
    }

    public void SelectButton(bool right){
        characterList[index].SetActive(false);
        if(right){
            index++;
            if(index == characterList.Length){
                index = 0;
            }
        } else {
            index--;
            if(index < 0){
                index = characterList.Length -1;  
            }
        }
        characterList[index].SetActive(true);
        
    }

    public void Select(){
        PlayerPrefs.SetInt("CharacterSelected", index);
        var characters = GameObject.FindGameObjectsWithTag("CharacterList");
        Debug.Log(characters[0].name);
        for(int i = 0; i< characters[0].transform.childCount; i++){
            if(characterList[index].name == characters[0].transform.GetChild(i).name){
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
