using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShipManager : NetworkBehaviour
{
    // Start is called before the first frame update
    private NetworkVariable<float> m_Oxygen = new NetworkVariable<float>(1f);
    private NetworkVariable<bool> m_Power = new NetworkVariable<bool>(true);
    public static ShipManager Instance;

    public bool HasPower{
        get => m_Power.Value;
    }
    protected void OnPowerChange(bool previous, bool current){
        
    }
    public override void OnNetworkSpawn(){
        m_Power.OnValueChanged += OnPowerChange;
    }
    public override void OnNetworkDespawn(){
        m_Power.OnValueChanged -= OnPowerChange;
    }

    private void TriggerPowerOutageEvent(){
        m_Power.Value = false;
    }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

}
