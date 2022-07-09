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

    Resolution[] resolutions;

    public TMPro.TMP_Dropdown resDD;


    void Start() {
        resolutions = Screen.resolutions;
        resDD.ClearOptions();
        List<string> resOpt = new List<string>();
        int resIdx = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            resOpt.Add(resolutions[i].width + "x" 
                        + resolutions[i].height 
                        + "@" + resolutions[i].refreshRate + "hz");
            //if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                resIdx = i;
            }
        }
        resDD.AddOptions(resOpt);
        resDD.value = resIdx;
        resDD.RefreshShownValue();
    }

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
                mode = 0;
                Resume();
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
                if (SceneManager.GetActiveScene().name != "MainMenu") {
                    PlayerManager.Instance.LocalPlayer.Avatar.LockMovement(GetHashCode());
                }
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
        if (SceneManager.GetActiveScene().name != "MainMenu") {
            PlayerManager.Instance.LocalPlayer.Avatar.ReleaseMovementLock(GetHashCode());
        }
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
        PopupText("Are you sure that you want to quit the game?");    
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
        Resume();
    }

    // Video Settings
    public void Video() {
        HideSettingUI();
        desc.text = "Video";
        SUIVideo.SetActive(true);
    }

    public void setQuality(int qualityIdx) {
        QualitySettings.SetQualityLevel(qualityIdx);
    }

    public void setFullscreen(bool yah) {
        Screen.fullScreen = yah;
    }

    public void setResIdx(int idx) {
        Resolution chosenRes = resolutions[idx];
        Screen.SetResolution(chosenRes.width, chosenRes.height, Screen.fullScreen);
    }

    // Audio Settings
    public void Audio() {
        HideSettingUI();
        desc.text = "Audio";
        SUIAudio.SetActive(true);
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

}
