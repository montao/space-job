using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerSpawnLocation : MonoBehaviour {

    public bool Occupied = false;

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1.0f);
    }

    public static Transform GetSpawn() {
        var spawns = FindObjectsOfType<PlayerSpawnLocation>();
        foreach (var spawn in spawns) {
            if (spawn.Occupied) {
                continue;
            }
            spawn.Occupied = true;
            return spawn.transform;
        }
        if (spawns.Length > 0) {
            Debug.LogWarning("All spawn locations are occupied.");
            return spawns[0].transform;
        } else {
            Debug.LogWarning("No spawn locations found.");
            return null;
        }
    }
}
