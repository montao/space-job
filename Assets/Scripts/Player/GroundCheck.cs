using UnityEngine;

public class GroundCheck : MonoBehaviour {
    public const float GROUND_CHECK_RADIUS = 0.1f;
    
    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, GROUND_CHECK_RADIUS);
    }
}
