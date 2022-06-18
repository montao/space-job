using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTerminalButton : InteractableBase
{
    public Door ConnectedDoor;
    public bool ShouldClose;
    //public WallTerminal DoorTerminal;
    protected override void Interaction(){
        ConnectedDoor.SetDoorStatusServerRpc(!ShouldClose);
        //DoorTerminal.DisplayDoorState(ShouldClose);
    }
}
