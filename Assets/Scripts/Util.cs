using UnityEngine;
using Unity.Netcode;

public class Util {
    public static Vector3 DivideElementwise(Vector3 a, Vector3 b) {
        var result = a;
        result.x /= b.x;
        result.y /= b.y;
        result.z /= b.z;
        return result;
    }

    public static bool NetworkObjectReferenceIsEmpty(NetworkObjectReference r) {
        NetworkObject _;
        return !r.TryGet(out _);
    }
}
