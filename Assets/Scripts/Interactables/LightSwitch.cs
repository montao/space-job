using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LightSwitch : NetworkBehaviour
{
    private SphereCollider m_SwitchCollider;
    private NetworkVariable<bool> m_LightStage = new NetworkVariable<bool>(true);
    private bool m_IsInArea = false;
    
    public List<Light> Lights;

    public override void OnNetworkSpawn(){
        m_LightStage.OnValueChanged += OnStateChange;
    }
    public override void OnNetworkDespawn(){
        m_LightStage.OnValueChanged -= OnStateChange;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SwitchServerRpc(){
        m_LightStage.Value = !m_LightStage.Value;
    }
    public void OnStateChange(bool previous, bool current){
        SetLightConditions(current);
    }

    void SetLightConditions(bool on){
        foreach(var light in Lights){
            light.gameObject.SetActive(on);
        }
    }
    private void Awake() {
        SetLightConditions(m_LightStage.Value);
        m_SwitchCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            Debug.Log("Local player entered");
            m_IsInArea = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            Debug.Log("Local player exit");
            m_IsInArea = false;
        }
    }
    void Update(){  
        if(m_IsInArea && Input.GetKeyDown(KeyCode.E)){
            SwitchServerRpc();
        }    
    }
}
