using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTerminalButton : InteractableBase
{
    public Door ConnectedDoor;
    public bool ShouldClose;
    protected override void Interaction(){
        ConnectedDoor.ToogleDoorStatusServerRpc();
    }
}
