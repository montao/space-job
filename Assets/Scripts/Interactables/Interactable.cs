using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Interactable<T> : NetworkBehaviour where T : unmanaged {
    private bool m_IsInArea = false;
    private NetworkVariable<T> m_State = new NetworkVariable<T>();
    public override void OnNetworkSpawn(){
        m_State.OnValueChanged += OnStateChange;
    }
    public override void OnNetworkDespawn(){
        m_State.OnValueChanged -= OnStateChange;
    }
    public abstract void OnStateChange(T previous, T current);
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(T value){
        m_State.Value = value;
    }
    private void OnTriggerEnter(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            m_IsInArea = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            m_IsInArea = false;
        }
    }
    protected abstract void Interaction();
    public T Value{
        get => m_State.Value;
    }
    void Update(){  
        if(m_IsInArea && Input.GetKeyDown(KeyCode.E)){
            Interaction();
        }
    }
}
