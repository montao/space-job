using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Interactable<T> : NetworkBehaviour where T : unmanaged {
    private bool m_IsInArea = false;
    protected NetworkVariable<T> m_State = new NetworkVariable<T>();
    public override void OnNetworkSpawn(){
        m_State.OnValueChanged += OnStateChange;
    }
    public override void OnNetworkDespawn(){
        m_State.OnValueChanged -= OnStateChange;
    }
    //will be called automatically for every client when new value is given.
    public abstract void OnStateChange(T previous, T current);
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
    public virtual void Update(){  
        if(m_IsInArea && Input.GetKeyDown(KeyCode.E)){
            Interaction();
        }
    }
}
