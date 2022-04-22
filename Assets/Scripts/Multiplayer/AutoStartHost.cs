using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class AutoStartHost : MonoBehaviour {

    private bool _playersSpawned = false;

    // Start is called before the first frame update
    void Start() {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = "127.0.0.1";
        transport.ConnectionData.ServerListenAddress = "127.0.0.1";
        NetworkManager.Singleton.StartHost();
        PlayerManager.Instance.LocalPlayerName = "Local Bean";
    }

    // Update is called once per frame
    void Update() {
        if (!_playersSpawned && PlayerManager.Instance.Players.Count > 0) {
            _playersSpawned = true;
            PlayerManager.SpawnAvatars();
        }
    }
}
