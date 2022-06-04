public class BreachBeGone : DroppableInteractable {
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        return PlayerAnimation.INTERACT;
    }
}
