using UnityEngine;

[ExecuteInEditMode]
public class HullBreachSpawnLocation : MonoBehaviour {
    private static Mesh GizmoMesh;

    void OnDrawGizmos() {
        var color_prev = Gizmos.color;
        Gizmos.color = Color.cyan;
        if (GizmoMesh == null) {
            Gizmos.DrawIcon(transform.position, "HullBreachIcon.png", true);
            GizmoMesh = GizmoMeshes.Instance.HullBreach;
        } else {
            Quaternion rot = transform.rotation * Quaternion.Euler(-90, 0, 0);
            Gizmos.DrawWireMesh(GizmoMesh, transform.position, rot, 2 * Vector3.one);
        }
        Gizmos.color = color_prev;
    }
}
