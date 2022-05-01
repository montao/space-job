using System;
using UnityEngine;
using Unity.Netcode;

public class ShipManager : NetworkBehaviour {
    public const char HAS_POWER = '\0';

    private NetworkVariable<float> m_Oxygen = new NetworkVariable<float>(1f);
    private NetworkVariable<char> m_Power = new NetworkVariable<char>(HAS_POWER);
    public static ShipManager Instance;

    public Terminal PowerTerminal;

    public bool HasPower{
        get => m_Power.Value == HAS_POWER;
    }
    protected void OnPowerChange(char _, char power){
        bool hasPower = power == HAS_POWER;
        LightManager.Instance.SetBackup(!hasPower);
        LightManager.Instance.SetNormal(hasPower);

        if (!hasPower) {
            PowerTerminal.DisplayError(
                    "Error:\n0x"
                    + Convert.ToByte(power ^ 'L').ToString("x2")
                    + Convert.ToByte(power).ToString("x2")
            );
        } else {
            PowerTerminal.DisplayError(":3");
        }

    }
    public override void OnNetworkSpawn(){
        m_Power.OnValueChanged += OnPowerChange;
        OnPowerChange(HAS_POWER, HAS_POWER);
    }
    public override void OnNetworkDespawn(){
        m_Power.OnValueChanged -= OnPowerChange;
    }

    private void TriggerPowerOutageEvent(){
        m_Power.Value = 'e';
    }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.P) && IsServer){
            if (HasPower) {
                TriggerPowerOutageEvent();
            } else {
                // Restore power
                m_Power.Value = HAS_POWER;
            }
        }
    }
}
