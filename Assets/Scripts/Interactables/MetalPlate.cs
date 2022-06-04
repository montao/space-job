public class MetalPlate : DroppableInteractable {
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        return PlayerAnimation.DRINK;
    }
}
