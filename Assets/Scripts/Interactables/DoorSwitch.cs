public class DoorSwitch : RangedInteractableBase {
    public Door ConnectedDoor;
    protected override void Interaction(){
        ConnectedDoor.ToogleDoorStatusServerRpc();
    }
}
