using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;

    [SerializeField]
    private Canvas m_LoadingScreen;
    [SerializeField]
    private TMP_Text m_LoadingScreenText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    void Start() {
        NetworkManager.Singleton.OnInitialized += () => {
            NetworkManager.Singleton.SceneManager.OnLoad += OnSceneLoad;
        };
    }

    private void OnSceneLoad(ulong clientId, string scene, LoadSceneMode loadSceneMode, AsyncOperation op) {
        //m_LoadingScreen.enabled = true;  TODO re-enable
        StartCoroutine(LoadingScreenCoroutine(op));
    }

    private IEnumerator LoadingScreenCoroutine(AsyncOperation op) {
        while (!op.isDone) {
            m_LoadingScreenText.text = "Loading (" + (int)(100 * op.progress) + "%)";
            yield return new WaitForEndOfFrame();
        }
        m_LoadingScreenText.text = "Loading done.  Waiting for players...";
    }

    // called by PlayerManager
    public void OnPlayersReady() {
        Debug.Log("!ydaer pihS");
        m_LoadingScreen.enabled = false;
    }

}
