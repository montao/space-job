using UnityEngine;
using Unity.Netcode;

public class Door : NetworkBehaviour {
    private Room m_ConnectedRoomA = null;
    private Room m_ConnectedRoomB = null;
    private NetworkVariable<bool> m_Open = new NetworkVariable<bool>(true);
    private Collider m_PhysicalDoor;
    private MeshRenderer m_PsychologicalDoor;
    private Animator m_DoorAnimator;


    public void SetRoom(Room room) {
        if (m_ConnectedRoomA == null) {
            m_ConnectedRoomA = room;
        } else if (m_ConnectedRoomB == null) {
            m_ConnectedRoomB = room;
        } else {
            Debug.Log("Door's full of rooms :(");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetRoomStatusServerRpc(){
        m_Open.Value = !m_Open.Value;
    }

    public override void OnNetworkSpawn(){
        m_Open.OnValueChanged += OnDoorStateChange;
    }
    public override void OnNetworkDespawn(){
        m_Open.OnValueChanged -= OnDoorStateChange;
    }

    public void OnDoorStateChange(bool previous, bool current){
        m_DoorAnimator.SetBool("open", current);
        //m_PsychologicalDoor.enabled = current;
        //m_PhysicalDoor.enabled = current;
    }

    public Room GetOtherRoom(Room thisRoom) {
        if (thisRoom == m_ConnectedRoomA) {
            return m_ConnectedRoomB;
        } else if (thisRoom == m_ConnectedRoomB) {
            return m_ConnectedRoomA;
        } else {
            Debug.LogWarning(
                    "Attempted to get other room for door " + name + " and room "
                    + thisRoom.name + ", but they're not connected!  Make sure to add this door to "
                    + thisRoom.name + " in the Editor."
            );
            return null;
        }
    }

    // Start is called before the first frame update
    void Start() {
        m_PhysicalDoor = GetComponent<Collider>();
        m_PsychologicalDoor = GetComponent<MeshRenderer>();
        m_DoorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
