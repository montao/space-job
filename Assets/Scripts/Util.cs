using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class Util {
    public static Vector3 DivideElementwise(Vector3 a, Vector3 b) {
        var result = a;
        result.x /= b.x;
        result.y /= b.y;
        result.z /= b.z;
        return result;
    }

    public static float Frac(float a) {
        return a - (int)a;
    }

    public static bool NetworkObjectReferenceIsEmpty(NetworkObjectReference r) {
        NetworkObject _;
        return !r.TryGet(out _);
    }

    public static T RandomChoice<T>(List<T> list) {
        if (list.Count < 0) {
            Debug.LogError("RandomChoice called for an empty list of " + typeof(T) + ".  This will crash, goodbye.");
        }

        int idx = Random.Range(0, list.Count);
        return list[idx];
    }
}
