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

    private List<Transform> m_HullBreachSpawnLocations = new List<Transform>();
    private List<HullBreachInstance> m_HullBreaches = new List<HullBreachInstance>();

    void Start() {
        ShipManager.Instance.Rooms.Add(this);
        foreach (Door door in Doors) {
            door.SetRoom(this);
        }
        m_RoomDisplay = GetComponentInChildren<TMP_Text>();

        foreach (HullBreachSpawnLocation spawnloc in GetComponentsInChildren<HullBreachSpawnLocation>()) {
            m_HullBreachSpawnLocations.Add(spawnloc.transform);
        }
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

    private void FixedUpdate() {
        if (IsServer) {
            float new_oxygen = m_RoomOxygen.Value;
            foreach (var breach in m_HullBreaches) {
                new_oxygen -= breach.DrainFactor() * Time.fixedDeltaTime * 0.03f;
            }
            new_oxygen += Time.fixedDeltaTime * 0.01f;
            m_RoomOxygen.Value = Mathf.Clamp(new_oxygen, 0f, 1f);
        }
    }

    public void SpawnHullBreach(EventParameters.HullBreachSize size) {
        if (!IsServer) {
            Debug.LogWarning("SpawnHullBreach should only be called on server!");
            Debug.Log("Spawned? " + IsSpawned);
            Debug.Log("Client? " + IsClient);
            return;
        }
        if (m_HullBreachSpawnLocations.Count == 0) {
            Debug.Log("You're in luck(?), there's no places to spawn hull breaches in " + name);
            return;
        }
        Transform location = Util.RandomChoice(m_HullBreachSpawnLocations);
        m_HullBreachSpawnLocations.Remove(location);
        GameObject breach = Instantiate(EventManager.Instance.HullBreachPrefab, location.position, location.rotation);
        breach.GetComponent<NetworkObject>().Spawn();
        breach.GetComponent<HullBreachInstance>().Setup(this, location);
        m_HullBreaches.Add(breach.GetComponent<HullBreachInstance>());
    }

    public void HullBreachResolved(HullBreachInstance breach, Transform freed_up_spawn_location) {
        m_HullBreaches.Remove(breach);
        m_HullBreachSpawnLocations.Add(freed_up_spawn_location);  // location availble again
    }
}
