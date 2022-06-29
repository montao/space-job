using UnityEngine;

[ExecuteInEditMode]
public class FireInstanceSpawnLocation : MonoBehaviour {
    private static Mesh GizmoMesh;

    void OnDrawGizmos() {
        var color_prev = Gizmos.color;
        Gizmos.color = Color.red;
        if (GizmoMesh == null) {
            Gizmos.DrawIcon(transform.position, "shittyFire.png", true);
            if (GizmoMeshes.Instance != null) {
                GizmoMesh = GizmoMeshes.Instance.Fire;
            }
        } else {
            Gizmos.DrawWireMesh(GizmoMesh, transform.position, transform.rotation, 2 * Vector3.one);
        }
        Gizmos.color = color_prev;
    }
}