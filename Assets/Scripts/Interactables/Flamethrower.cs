public class Flamethrower : DroppableInteractable {
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        return PlayerAnimation.INTERACT;
    }
}
