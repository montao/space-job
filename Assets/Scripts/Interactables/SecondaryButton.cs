public abstract class SecondaryButton : InteractableBase {
    public bool CanInteract;

    protected override bool PlayerCanInteract() {
        return CanInteract;
    }
}
