using Unity.Netcode;
using UnityEngine;

public abstract class InteractableBase : NetworkBehaviour {

    protected LayerMask m_HighlightedLayer;
    protected LayerMask m_DefaultLayer;
    protected MeshRenderer m_Renderer;
    public bool NeedsPower = false;
    public PlayerAnimation TriggeredAnimation = PlayerAnimation.INTERACT;

    // Called when item is held in hand and right mouse button pressed
    // Returns animation to play upon interaction
    public virtual PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        return PlayerAnimation.NONE;  // do nothing by default
    }

    protected abstract void Interaction();
    protected abstract bool PlayerCanInteract();

    // Relevant only for HOLD_DOWN mode.  Called when mouse button is lifted.
    protected virtual void StopInteraction() {}

    public virtual void Start() {
        m_HighlightedLayer = LayerMask.NameToLayer("Highlighted");
        m_Renderer = GetComponent<MeshRenderer>();
        if (!m_Renderer) {
            m_Renderer = GetComponentInChildren<MeshRenderer>();
        }
        if (!m_Renderer) {
            m_Renderer = GetComponentInParent<MeshRenderer>();
        }

        if (m_Renderer) {
            m_DefaultLayer = m_Renderer.gameObject.layer;
        } else {
            Debug.LogWarning("No renderer found for " + gameObject.name);
        }
    }

    protected void SetHighlight(bool highlighted) {
        m_Renderer.gameObject.layer = highlighted ? m_HighlightedLayer : m_DefaultLayer;
    }


    protected void OnMouseOver() {
        SetHighlight(PlayerCanInteract());

        if (PlayerCanInteract() && (!NeedsPower || ShipManager.Instance.HasPower)) {
            bool playAnim = false;
            if (Input.GetButtonDown("Fire1")) {
                Interaction();
                playAnim = true;
            } else if (Input.GetButtonUp("Fire1")){
                StopInteraction();
                playAnim = true;
            }

            if (playAnim) {
                PlayerManager.Instance.LocalPlayer.Avatar.AnimationController.TriggerAnimation(TriggeredAnimation);
            }
        }
    }
    protected void OnMouseExit() {
        SetHighlight(false);
        StopInteraction();
    }

    public virtual void Update() {
    }

}
