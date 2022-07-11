public class DestinationButton : InteractableBase {
    private int m_DestinationIndex;
    public int DestinationIndex {
        get => m_DestinationIndex;
        set => m_DestinationIndex = value;
    }

    protected override void Interaction() {
        ShipManager.Instance.SetDestinationIndexServerRpc(DestinationIndex);
    }

    public override string FriendlyName() {
        return "Destination #" + DestinationIndex;
    }

}
