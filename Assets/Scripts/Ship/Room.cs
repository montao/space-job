using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
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

    [SerializeField]
    private List<Transform> m_HullBreachSpawnLocations = new List<Transform>();
    private List<GameObject> m_HullBreaches = new List<GameObject>();

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
        if(m_RoomDisplay == null) return;
        m_RoomDisplay.text = text;
    }

    private void Update() {
        DisplayText("Oxygen Capacity: " + RoomOxygen);
    }

    private void FixedUpdate() {
        // m_RoomOxygen -= m_HullBreaches.Count * Time.fixedTimeDelta;
    }

    public void SpawnHullBreach(EventParameters.HullBreachSize size) {
        if (m_HullBreachSpawnLocations.Count == 0) {
            Debug.Log("You're in luck(?), there's no places to spawn hull breaches in " + name);
            return;
        }
        Transform location = Util.RandomChoice(m_HullBreachSpawnLocations);
        m_HullBreachSpawnLocations.Remove(location);
        GameObject breach = Instantiate(EventManager.Instance.HullBreachPrefab, location.position, location.rotation);
        m_HullBreaches.Add(breach);
    }

    public void HullBreachResolved(HullBreachInstance breach) {
        m_HullBreachSpawnLocations.Add(breach.transform);
    }
}
