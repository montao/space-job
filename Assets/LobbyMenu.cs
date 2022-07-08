using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;


public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private TMP_Text desc;
    public static bool MenuIsOpen = false;
    public static bool PopupIsOpen = false;

    static bool PopupBool = false;
    static int mode = 0; 

    public GameObject MenuUI;
    public GameObject PopupUI;

    public GameObject SettingsUI;

    public GameObject UIBackground;


    public GameObject SUIAudio;
    public GameObject SUIVideo;
    public GameObject SUIControls;
    public GameObject SUITips;

    public AudioMixer mix;


    // Update is called once per frame
    void Update()
    {
        if (PopupBool) {
            PopupUI.SetActive(false);
            SettingsUI.SetActive(false);
            PopupIsOpen = false;
            ShowUI();
            if (mode == 1) {
                Debug.Log("bye");
                Application.Quit();
            }
            else if (mode == 2) {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                NetworkManager.Singleton.Shutdown();
            }
        }

        // todo for controller
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (PopupIsOpen) {
                PopupIsOpen = false;
                SettingsUI.SetActive(false);
                PopupUI.SetActive(false);
                mode = 0;
                ShowUI();
            }
            else if (MenuIsOpen) {
                Resume();
            }
            else {
                UIBackground.SetActive(true);
                ShowUI();
            }
        }
    }

    public void HideUI() {
        MenuUI.SetActive(false);
        MenuIsOpen = false;
    }

    public void Resume() {
        HideUI();
        UIBackground.SetActive(false);
    }

    public void ShowUI() {
        MenuUI.SetActive(true);
        MenuIsOpen = true;
    }

    public void LoadMenu() {
        mode = 2;
        PopupText("are you sure that you want to return to the menu?");    
    }

    public void OpenSettings() {
        mode = 3;
        HideUI();
        PopupIsOpen = true;
        SettingsUI.SetActive(true);
        Control();
    }

    public void CloseSettings() {
        mode = 0;
        PopupIsOpen = false;
        SettingsUI.SetActive(false);
        ShowUI();
    }

    public void QuitGame() {
        mode = 1;
        PopupText("are you sure that you want to quit the game?");    
    }

    public void PopupText(string message) {
        text.text = message;
        PopupIsOpen = true;
        HideUI();
        PopupUI.SetActive(true);
    }

    public void Decline() {
        PopupUI.SetActive(false);
        PopupIsOpen = false;
        mode = 0;
        ShowUI();
    }

    public void Accept() {
        PopupBool = true;
    }

    public void Video() {
        HideSettingUI();
        desc.text = "Video";
        SUIVideo.SetActive(true);
    }

    public void Audio() {
        HideSettingUI();
        desc.text = "Audio";
        SUIAudio.SetActive(true);
    }

    public void Tips() {
        HideSettingUI();
        desc.text = "Tips";
        SUITips.SetActive(true);
    }

    public void Control() {
        HideSettingUI();
        desc.text = "Controls";
        SUIControls.SetActive(true);
    }

    public void HideSettingUI() {
        SUIAudio.SetActive(false);
        SUIVideo.SetActive(false);
        SUIControls.SetActive(false);
        SUITips.SetActive(false);

    }

    public void SetMasterVol(float vol) {
        mix.SetFloat("MasterVol", vol);
    }

    public void SetMusicVol(float vol) {
        mix.SetFloat("MusicVol", vol);
    }
    public void SetSoundVol(float vol) {
        mix.SetFloat("SoundVol", vol);
    }
    public void SetAmbientVol(float vol) {
        mix.SetFloat("AmbientVol", vol);
    }
    
}
