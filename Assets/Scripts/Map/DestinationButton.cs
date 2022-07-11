using UnityEngine;

public class DestinationButton : InteractableBase {
    private int m_DestinationIndex;
    public int DestinationIndex {
        get => m_DestinationIndex;
        set => m_DestinationIndex = value;
    }

    [SerializeField]
    private MeshRenderer m_CircleRenderer;

    public void SetCircleEnabled(bool enabled) {
        m_CircleRenderer.enabled = enabled;
    }

    protected override void Interaction() {
        ShipManager.Instance.SetDestinationIndexServerRpc(DestinationIndex);
    }

    public override string FriendlyName() {
        return "Destination #" + DestinationIndex;
    }

}
