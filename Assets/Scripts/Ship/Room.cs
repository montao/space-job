using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using TMPro;

public class Room : NetworkBehaviour {
    public string Name;
    public List<Door> Doors = new List<Door>();
    [SerializeField]
    private NetworkVariable<float> m_RoomOxygen = new NetworkVariable<float>(1.0f);
    public float RoomOxygen {
        get => m_RoomOxygen.Value;
        set => m_RoomOxygen.Value = value;
    }
    private TMP_Text m_RoomDisplay;

    void Awake() {
        ShipManager.Instance.Rooms.Add(this);
        foreach (Door door in Doors) {
            door.SetRoom(this);
        }
        m_RoomDisplay = GetComponentInChildren<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        if (player != null) {
            player.CurrentRoom = this;
        }
    }

    public void DisplayText(string text) {
        if (m_RoomDisplay != null) {
            m_RoomDisplay.text = text;
        }
    }

    private void Update() {
        DisplayText("Oxygen Capacity: " + RoomOxygen);
    }

}
