using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkObject))]
public class PersistentPlayer : NetworkBehaviour {

    public delegate void OnAvatarChangedDelegate(PlayerAvatar avatar);
    public OnAvatarChangedDelegate OnAvatarChanged;

    private NetworkVariable<FixedString32Bytes> m_PlayerName
            = new NetworkVariable<FixedString32Bytes>(default, default, NetworkVariableWritePermission.Owner);
    public string PlayerName {
        get {
            return m_PlayerName.Value.ToString();
        }
        set {
            if (IsOwner) {
                m_PlayerName.Value = value;
            } else {
                Debug.LogWarning("Cannot set name unless owner");
            }
        }
    }

    [SerializeField]
    private GameObject m_AvatarPrefab;

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

        // jah, a bit hacky but seems to do the trick?
        m_PlayerName.OnValueChanged += (FixedString32Bytes _, FixedString32Bytes __) => {
            if (Avatar != null) {
                Avatar.Setup();
            }
        };
    }

    public void AvatarChanged(NetworkObjectReference previous, NetworkObjectReference current) {
        Debug.Log("Avatar changed for " + PlayerName);
        if (OnAvatarChanged != null) {
            OnAvatarChanged(Avatar);
        }
    }

    public override void OnNetworkSpawn() {
        Debug.Log("hewwo");
        m_Avatar.OnValueChanged += AvatarChanged;
    }

    public override void OnNetworkDespawn() {
        m_Avatar.OnValueChanged -= AvatarChanged;
    }

    // Note: Only called by Server, as they are the only one allowed to spawn objects
    public void SpawnAvatar(Transform spawnLocation) {
        Debug.Log("Spawning an avatar!");

        var owner = OwnerClientId;

        PlayerAvatar avatar = GameObject.Instantiate(m_AvatarPrefab, spawnLocation.position, spawnLocation.rotation).GetComponent<PlayerAvatar>();
        NetworkObject avatarNetworkObject = avatar.GetComponent<NetworkObject>();
        avatarNetworkObject.SpawnWithOwnership(owner);
        //avatar.OnAvatarSpawnedClientRpc();

        // I proudly present: Jank
        if (SceneManager.GetActiveScene().name == "SampleScene") {
            avatar.transform.GetChild(0).localScale = new Vector3(2.5f, 2.5f, 2.5f);
            CharacterController characterController = avatar.GetComponent<CharacterController>();
            characterController.height = 4;
            characterController.center = new Vector3(0, 1, 0);
            foreach (var rectt in avatar.GetComponentsInChildren<RectTransform>()) {
                if (rectt.name == "PlayerName") {
                    rectt.position = rectt.position + new Vector3(0, 2.7f, 0);
                }
            }
        }

        m_Avatar.Value = avatarNetworkObject;

    }
}
