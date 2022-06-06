using UnityEngine;

[ExecuteInEditMode]
public class GizmoMeshes : MonoBehaviour {

    public Mesh HullBreach;

    public static GizmoMeshes Instance;

    void Update() {
#if UNITY_EDITOR
        if (Instance == null) {
            Instance = this;
        }
#endif
    }
}
