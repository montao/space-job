using System.Collections;
using System.Collections.Generic;
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

    private PlayerAvatar avatar;
    public PlayerAvatar Avatar {
        get {
            return avatar;
        }
    }

    void Start() {
        PlayerManager.Instance.RegisterPlayer(this, IsOwner);
    }

    // TODO:  Since this is only called by the host, any setup we do, i.e. setting the `avatar` member
    // will not be reflected by clients.  To fix this, it'd probably make sense to make `avatar` a
    // `NetworkObject<NetworkObjectReference>` (https://docs-multiplayer.unity3d.com/netcode/current/api/Unity.Netcode.NetworkObjectReference)
    // or implement some logic to let each avatar find the corresponsing
    // PersistentPlayer (or the other way round).  Anyway, I'm going to sleep
    // now, good night~
    public void SpawnAvatar(Transform spawnLocation) {
        var owner = OwnerClientId;

        avatar = GameObject.Instantiate(avatarPrefab, spawnLocation).GetComponent<PlayerAvatar>();
        avatar.GetComponent<NetworkObject>().Spawn();
        avatar.GetComponent<NetworkObject>().ChangeOwnership(owner);
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
