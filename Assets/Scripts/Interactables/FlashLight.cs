using UnityEngine;
using Unity.Netcode;

public class FlashLight : DroppableInteractable {

    private Light m_Beam;
    private FlashLightInHand m_InHand;

    private NetworkVariable<bool> m_TurnedOn = new NetworkVariable<bool>(true);
    private NetworkVariable<Quaternion> m_RotationInHand = new NetworkVariable<Quaternion>();

    public override void Awake() {
        base.Awake();
        m_Beam = GetComponentInChildren<Light>();

        // Only run on local client
        OnPickup += (PlayerAvatar avatar) => {
            Debug.Log("Flashlight picked up by " + avatar.name);
            var fl_hand = avatar.GetComponentInChildren<FlashLightInHand>(includeInactive: true);
            Debug.Log("FlashLightInHand: " + fl_hand);
            fl_hand.gameObject.SetActive(true);
            fl_hand.ItemRenderer = avatar.PrimaryItemDisplay;
            m_Beam.enabled = false;
        };

        // Only run on local client
        OnDrop -= (PlayerAvatar avatar) => {
            avatar.GetComponentInChildren<FlashLightInHand>().gameObject.SetActive(false);
            m_Beam.enabled = true;
        };
    }

    public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            m_State.OnValueChanged += OnHolderChanged;
            m_TurnedOn.OnValueChanged += OnTurnedOnChanged;
    }

    public override void OnNetworkDespawn() {
            base.OnNetworkDespawn();
            m_State.OnValueChanged -= OnHolderChanged;
            m_TurnedOn.OnValueChanged -= OnTurnedOnChanged;
    }

    public void OnHolderChanged(int prev, int curr) {
        if (curr == DroppableInteractable.IN_WORLD) {  // dropped
            m_Beam.enabled = true;
        } else {  // picked up
            m_Beam.enabled = false;
        }
    }

    public void OnRotatationUpdated(Quaternion _, Quaternion rot) {
        if (LocalClientIsHolding()) {
            return;
        }
    }

    public bool LocalClientIsHolding() {
        return (int)NetworkManager.Singleton.LocalClientId == m_State.Value;
    }

    [ServerRpc]
    public void SetTurnedOnServerRpc(bool turned_on) {
        // TODO, for now always on
    }

    public void OnTurnedOnChanged(bool _, bool turned_on) {
        // TODO
    }

    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        /* TODO
        m_TurnedOn = !m_TurnedOn;
        m_Beam.enabled = m_TurnedOn;
        AudioClip sound = GetRandomInteractionClip();
        audioSource.PlayOneShot(sound);
        */
        return PlayerAnimation.INTERACT;
    }
}
