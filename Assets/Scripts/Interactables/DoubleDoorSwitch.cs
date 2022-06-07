using UnityEngine;

[RequireComponent(typeof(Door))]
public class DoubleDoorSwitch : RangedInteractableBase {

    private Door m_Door;

    void Awake() {
        m_Door = GetComponent<Door>();
    }

    protected override void Interaction() {
        m_Door.ToogleDoorStatusServerRpc();
    }
}
