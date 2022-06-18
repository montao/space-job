using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlashLightInHand : MonoBehaviour {

    public static float MAX_RAYCAST_DIST = 10000f;
    public Transform ItemRenderer;  // set by FlashLight in OnPickup
    public bool TrackingActive = false;

    Vector3? GetPointerTarget() {
        RaycastHit hit;
        Ray ray = CameraBrain.Instance.OutputCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, MAX_RAYCAST_DIST)) {
            return hit.point;
        }

        return null;
    }

    void Update() {
        if (!TrackingActive) {
            return;
        }
        var target = GetPointerTarget();
        if (target.HasValue) {
            ItemRenderer.LookAt(transform.position - (target.Value - transform.position));
        }

    }

    void OnDrawGizmos() {
        if (!CameraBrain.Instance) {
            return;
        }
        var target = GetPointerTarget();
        if (target.HasValue) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(target.Value, 0.05f);
        }
    }
}
