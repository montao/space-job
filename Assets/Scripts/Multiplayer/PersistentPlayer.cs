using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

[RequireComponent(typeof(NetworkObject))]
public class PersistentPlayer : NetworkBehaviour {

    private NetworkVariable<FixedString32Bytes> playerName
            = new NetworkVariable<FixedString32Bytes>();
    public string PlayerName {
        get {
            return playerName.Value.ToString();
        }
        set {
            Debug.Log("Name changed");
            if (IsOwner) {
                SetNameServerRpc(value);
            }
        }
    }

    [SerializeField]
    private GameObject avatarPrefab;

    private NetworkVariable<NetworkObjectReference> m_Avatar
            = new NetworkVariable<NetworkObjectReference>();
    public PlayerAvatar Avatar {
        get {
            NetworkObject ava;
            if (m_Avatar.Value.TryGet(out ava)) {
                return ava.GetComponent<PlayerAvatar>();
            }
            return null;
        }
    }

    void Start() {
        PlayerManager.Instance.RegisterPlayer(this, IsOwner);
    }

    public void SpawnAvatar(Transform spawnLocation) {
        var owner = OwnerClientId;

        PlayerAvatar avatar = GameObject.Instantiate(avatarPrefab, spawnLocation).GetComponent<PlayerAvatar>();
        NetworkObject avatarNetworkObject = avatar.GetComponent<NetworkObject>();
        avatarNetworkObject.Spawn();
        avatarNetworkObject.ChangeOwnership(owner);

        m_Avatar.Value = avatarNetworkObject;
    }

    // Update is called once per frame
    void Update() {
        
    }

    [ServerRpc]
    public void SetNameServerRpc(string name) {
        Debug.Log("SetNameServerRpc " + name);
        playerName.Value = name;
    }
}
