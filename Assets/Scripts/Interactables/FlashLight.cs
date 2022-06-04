using UnityEngine;

public class FlashLight : DroppableInteractable {

    private Light m_Beam;
    private bool m_TurnedOn = true;

    public override void Awake() {
        base.Awake();
        m_Beam = GetComponent<Light>();
    }

    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        m_TurnedOn = !m_TurnedOn;
        m_Beam.enabled = m_TurnedOn;
        return PlayerAnimation.INTERACT;
    }
}
