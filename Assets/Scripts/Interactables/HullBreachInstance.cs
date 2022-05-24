using UnityEngine;

public class HullBreachInstance : RangedInteractableBase {

    private Room m_Room;

    protected override void Interaction() {
        // TODO
    }

    public void Setup(Room room) {
        m_Room = room;
    }

    public void Resolved() {
        m_Room.HullBreachResolved(this);
    }
}
