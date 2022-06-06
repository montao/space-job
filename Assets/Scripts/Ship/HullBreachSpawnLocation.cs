using UnityEngine;

[ExecuteInEditMode]
public class HullBreachSpawnLocation : MonoBehaviour {
    private static Mesh GizmoMesh;

    void OnDrawGizmos() {
        var color_prev = Gizmos.color;
        Gizmos.color = Color.cyan;
        if (GizmoMesh == null || GizmoMeshes.Instance == null) {
            Gizmos.DrawIcon(transform.position, "HullBreachIcon.png", true);
            GizmoMesh = GizmoMeshes.Instance.HullBreach;
        } else {
            Gizmos.DrawWireMesh(GizmoMesh, transform.position, transform.rotation, 2 * Vector3.one);
        }
        Gizmos.color = color_prev;
    }
}
