public class DropPod : RangedInteractableBase {
    public Door ConnectedDoor;
    protected override void Interaction(){
        ConnectedDoor.ToogleDoorStatusServerRpc();
    }

    public override string FriendlyName() {
        return "CargoDropPod";
    }
}
