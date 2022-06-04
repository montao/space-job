public class PipeWrench : DroppableInteractable{
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        return PlayerAnimation.INTERACT;
    }
}
