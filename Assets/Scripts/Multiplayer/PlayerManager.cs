using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour {

    public static PlayerManager Instance;

    public string LocalPlayerName = "";

    private NetworkVariable<int> _playerCount =
            new NetworkVariable<int>(1);

    private List<PersistentPlayer> _players;
    public List<PersistentPlayer> Players {
        get => _players;
    }

    private PersistentPlayer _localPlayer;
    public PersistentPlayer LocalPlayer {
        get => _localPlayer;
    }

    public string LobbyInfo {
        get {
            string info = _playerCount.Value + "/4: ";
            int n = 0;
            foreach (var player in _players) {
                info = info + ((n++ == 0) ? "" : ", ") + player.PlayerName;
            }
            return info;
        }
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        _players = new List<PersistentPlayer>();
    }

    void Start() {
        NetworkManager.Singleton.OnClientConnectedCallback +=
            (id) => {
                if (IsServer) {
                    Debug.Log("New player " + id);
                    _playerCount.Value++;
                }
            };

        NetworkManager.Singleton.OnClientDisconnectCallback +=
            (id) => {
                if (IsServer) {
                    _playerCount.Value--;
                }
            };
    }

    void Update() {

    }

    public void RegisterPlayer(PersistentPlayer player, bool isLocal) {
        _players.Add(player);
        if (isLocal) {
            player.gameObject.transform.position = transform.position;
            player.gameObject.transform.rotation = transform.rotation;
            _localPlayer = player;
            _localPlayer.PlayerName = LocalPlayerName;
        }
    }

}
