using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;

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
        StartCoroutine(LoadingScreenCoroutine(op));
    }

    private IEnumerator LoadingScreenCoroutine(AsyncOperation op) {
        while (!op.isDone) {
            Debug.Log("Loading... " + op.progress + "%");
            yield return new WaitForEndOfFrame();
        }
    }
}
