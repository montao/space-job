using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollision : MonoBehaviour {
    void Start() {
        Physics.IgnoreLayerCollision(6, 7);
    }
}
