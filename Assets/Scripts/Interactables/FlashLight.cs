using UnityEngine;

public class FlashLight : DroppableInteractable {

    private Light m_Beam;
    private bool m_TurnedOn = true;

    public override void Awake() {
        base.Awake();
        m_Beam = GetComponent<Light>();

        OnDrop = (PlayerAvatar avatar) => {
            avatar.GetComponentInChildren<FlashLightInHand>().gameObject.SetActive(false);
            m_Beam.enabled = true;
        };
        OnPickup = (PlayerAvatar avatar) => {
            var fl_hand = avatar.GetComponentInChildren<FlashLightInHand>(includeInactive: true);
            fl_hand.gameObject.SetActive(true);
            fl_hand.ItemRenderer = avatar.PrimaryItemDisplay;
            m_Beam.enabled = false;
        };
    }

    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        m_TurnedOn = !m_TurnedOn;
        m_Beam.enabled = m_TurnedOn;
        AudioClip sound = GetRandomInteractionClip();
        audioSource.PlayOneShot(sound);
        return PlayerAnimation.INTERACT;
    }
}
