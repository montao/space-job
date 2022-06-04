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
    public virtual int SelfInteraction(PlayerAvatar avatar) {
        return -1;
    }

    protected abstract void Interaction();
    protected abstract bool PlayerCanInteract();

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
        if (PlayerCanInteract() && Input.GetButtonDown("Fire1") && (!NeedsPower || ShipManager.Instance.HasPower)) {
            Interaction();
            PlayerManager.Instance.LocalPlayer.Avatar.AnimationController.TriggerAnimationClientRpc(TriggeredAnimation);
        }
    }
    protected void OnMouseExit() {
        SetHighlight(false);
    }

    public virtual void Update() {
    }

}
