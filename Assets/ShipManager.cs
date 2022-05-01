using System;
using UnityEngine;
using Unity.Netcode;

public class ShipManager : NetworkBehaviour {
    public const char HAS_POWER = '\0';

    private NetworkVariable<float> m_Oxygen = new NetworkVariable<float>(1f);
    private NetworkVariable<char> m_Power = new NetworkVariable<char>(HAS_POWER);
    public static ShipManager Instance;

    public static char[] ERROR_CODES = {'2', 'e', (char)0xba, (char)42, '\n'};

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
                    "0x"
                    + Convert.ToByte(power ^ 'L').ToString("x2").ToUpper()
                    + Convert.ToByte(power).ToString("x2").ToUpper()
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

    public void TriggerPowerOutageEvent(){
        int error_idx = UnityEngine.Random.Range(0, ERROR_CODES.Length - 1);
        m_Power.Value = ERROR_CODES[error_idx];
    }
    public void ResolvePowerOutageEvent() {
        m_Power.Value = HAS_POWER;
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
                ResolvePowerOutageEvent();
            }
        }
    }
}
