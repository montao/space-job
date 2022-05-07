using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * TODO separate out the connection stuff
 */
public class MainMenu : MonoBehaviour {

    private NetworkManager networkManager;
    private UnityTransport transport;
    private bool connected = false;
    private bool host = false;
    private bool ishosted = false;

    [SerializeField]
    private TMP_InputField playerName;
    [SerializeField]
    private TMP_InputField serverAddress;
    [SerializeField]
    private TMP_Text lobbyInfo;
    [SerializeField]
    private GameObject startClient;
    [SerializeField]
    private TMP_Text startClientTMP;
    [SerializeField]
    private string hostName;

    void Start() {
        networkManager = NetworkManager.Singleton;
        transport = networkManager.GetComponentInParent<UnityTransport>();
        
        startClient = GameObject.Find("Start Client");
        startClientTMP = startClient.transform.GetChild(0).GetComponent<TMP_Text>();
        startClient.SetActive(false);

        serverAddress.text = MultiplayerUtil.GetLocalIPAddress();
        ServerAddressChanged();

        playerName.text = SystemInfo.deviceName;
        PlayerNameChanged();

        if (IsArgon()) {
            serverAddress.text = "192.168.0.10";
            ServerAddressChanged();
        }
    }

    void Update() {
        if(!host){
            if(ishosted){
                startClient.SetActive(true);
                startClientTMP.text = hostName + " just hosted a game. Join!"; 
            }
            
        }

    }

    public void PlayerNameChanged() {
        string name = playerName.text;
        PlayerManager.Instance.LocalPlayerName = name;
    }

    public void ServerAddressChanged() {
        string ip = serverAddress.text;
        transport.ConnectionData.Address = ip;
        transport.ConnectionData.ServerListenAddress = ip;
    }

    public bool IsArgon() {
        return SystemInfo.deviceName == "argon";
    }

    public void StartHost() {
        if (MultiplayerUtil.GetLocalIPAddress() != serverAddress.text && !IsArgon()) {
            Debug.LogWarning("Cannot host a game from a different ip address");
            return;
        }

        connected = connected || NetworkManager.Singleton.StartHost();
        host = true;
        ishosted = true;
        hostName = PlayerManager.Instance.LocalPlayerName;
    }

    public void Reload() {
        if (MultiplayerUtil.GetLocalIPAddress() == serverAddress.text && !IsArgon()) {
            Debug.LogWarning("Cannot join a game from the same ip address, host instead");

            return;
        }

        connected = connected || NetworkManager.Singleton.StartClient();
        ishosted = true;

        /* startClient.SetActive(true);
        startClientTMP.text = playerName.text + " just hosted a game. Join!";
 */
        
    }

    public void StartClient() {
        if (MultiplayerUtil.GetLocalIPAddress() == serverAddress.text && !IsArgon()) {
            Debug.LogWarning("Cannot join a game from the same ip address, host instead");
            return;
        }
        
        //connected = connected || NetworkManager.Singleton.StartClient();
        StartLobby();
    }

    public void StartGame() {
        if (!connected) {
            Debug.LogWarning("Cannot start game unless connected!");
            return;
        }
        if (!host) {
            Debug.LogWarning("Cannot start game as non-host!");
            return;
        }

        if (host) {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.SpawnAvatars;
            if (playerName.text != "Demo") {
                NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            }
            else {
                NetworkManager.Singleton.SceneManager.LoadScene("ShipScene", LoadSceneMode.Single);
            }
        }
        
    }

    public void StartLobby() {
        if (!connected) {
            Debug.LogWarning("Cannot join game unless connected!");
            return;
        }
        if (!host) {
            Debug.LogWarning("Cannot host game as non-host!");
            return;
        }

        if (host) {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.SpawnAvatars;
            if (playerName.text != "Demo") {
                NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            }
            else {
                NetworkManager.Singleton.SceneManager.LoadScene("ShipScene", LoadSceneMode.Single);
            }
        }
        
    }

    void OnGUI() {
        // TODO only update when needed
        if (connected) {
            lobbyInfo.text = PlayerManager.Instance.LobbyInfo;
        } else {
            lobbyInfo.text = "Not connected";
        }
    }
}
