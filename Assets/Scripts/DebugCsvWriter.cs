using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DebugCsvWriter : MonoBehaviour {
    float ValueA() {
        return ShipManager.Instance.Rooms[0].RoomOxygen;
    }
    float ValueB() {
        return ShipManager.Instance.Rooms[1].RoomOxygen;
    }

    private List<string> lines = new List<string>();

    void FixedUpdate() {
        if (Application.isEditor) {
            string line = Time.fixedTime + "," + ValueA() + "," + ValueB();
            lines.Add(line);
        }
    }

    void OnDestroy() {
        if (Application.isEditor) {
            var f = File.CreateText("/tmp/log.csv");
            foreach (string line in lines) {
                f.WriteLine(line);
            }
            f.Close();
        }
    }
}
