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
    private UnityTransport m_Transport;
    private bool connected = false;
    private bool host = false;
    private bool ishosted = false;
    private bool clientLobby = false;
    [SerializeField]
    private TMP_InputField playerName;
    [SerializeField]
    private TMP_InputField serverAddress;
    [SerializeField]
    private TMP_Text lobbyInfo;
    [SerializeField]
    private GameObject hostGame;
    private GameObject startClient;
    [SerializeField]
    private TMP_Text startClientTMP;
    private GameObject startHost;
    [SerializeField]
    private TMP_Text startHostTMP;

    

    void Start() {
        networkManager = NetworkManager.Singleton;
        m_Transport = networkManager.GetComponentInParent<UnityTransport>();
        
        hostGame = GameObject.Find("Start Host");
        hostGame.SetActive(true);

        startHost = GameObject.Find("Start Game");
        startHostTMP = startHost.transform.GetChild(0).GetComponent<TMP_Text>();
        startHost.SetActive(false);

        startClient = GameObject.Find("Start Client");
        startClientTMP = startClient.transform.GetChild(0).GetComponent<TMP_Text>();
        startClient.SetActive(false);

        serverAddress.text = MultiplayerUtil.GetLocalIPAddress();
        var envvar = System.Environment.GetEnvironmentVariable("SPACEBULLAR_HOST");
        if (envvar != null) {
            serverAddress.text = envvar;
        }
        ServerAddressChanged();

        playerName.text = "";
        PlayerNameChanged();

        if (IsArgon()) {
            serverAddress.text = "192.168.0.10";
            ServerAddressChanged();
        }
#if UNITY_SERVER
        StartHost();
#endif
    }

    void Update() {
        if(!host){
            if(ishosted){

                Debug.Log(PlayerManager.Instance.LobbyInfo);
                startClient.SetActive(true);
                startClientTMP.text = "Click to join Lobby " + serverAddress.text; 
            }
            
        }
        if(host){
            hostGame.SetActive(false);
            startHost.SetActive(true);
            startHostTMP.text = "Start Lobby!"; 
        }
#if UNITY_SERVER
        if (PlayerManager.Instance.ConnectedPlayerCount > 2) {
            StartGame();
        }
#endif
    }

    public void PlayerNameChanged() {
        string name = playerName.text;
        PlayerManager.Instance.LocalPlayerName = name;
    }

    public void ServerAddressChanged() {
        string ip = serverAddress.text;
        m_Transport.ConnectionData.Address = ip;
        m_Transport.ConnectionData.ServerListenAddress = ip;
    }

    public bool IsArgon() {
        return SystemInfo.deviceName == "argon" && System.Environment.GetEnvironmentVariable("GOOSE") == "y";
    }

    public void StartHost() {
        if (MultiplayerUtil.GetLocalIPAddress() != serverAddress.text) {
            Debug.LogWarning("Cannot host a game from a different ip address");
            return;
        }

        connected = connected || NetworkManager.Singleton.StartHost();
        if (connected) {
            host = true;
            ishosted = true;
        } else {
            m_Transport.Shutdown();
        }
    }

    public void Reload() {
        // not great since this is set every reload but works fpor now
        ishosted = true;
        Debug.Log("Reload");
        
    }

    public void StartClient() {
        connected = connected || NetworkManager.Singleton.StartClient();

        if (!connected) {
            m_Transport.Shutdown();
        }
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
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.Instance.StartShip;
            if (playerName.text != "Demo") {
                NetworkManager.Singleton.SceneManager.LoadScene("ShipScene", LoadSceneMode.Single);

            }
            else {
                NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
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
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.Instance.StartShip;
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
            if(PlayerManager.Instance.LobbyInfo == "1/4: " && MultiplayerUtil.GetLocalIPAddress() != serverAddress.text) {
                lobbyInfo.text = "waiting for the host to connect";
            } 
            else{
                lobbyInfo.text = PlayerManager.Instance.LobbyInfo;
            }
        } else {
            lobbyInfo.text = "Not connected";
        }
    }
}
