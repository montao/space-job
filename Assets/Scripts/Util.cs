using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
    public static Vector3 DivideElementwise(Vector3 a, Vector3 b) {
        var result = a;
        result.x /= b.x;
        result.y /= b.y;
        result.z /= b.z;
        return result;
    }
}
