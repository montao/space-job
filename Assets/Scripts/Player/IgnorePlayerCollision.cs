using UnityEngine;

public class IgnorePlayerCollision : MonoBehaviour {
    void Start() {
        Physics.IgnoreLayerCollision(2, 7);
    }
}
