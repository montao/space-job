using UnityEngine;

public class SpaceCamera : MonoBehaviour {

    private Camera m_Camera;

    void Awake() {
        m_Camera = GetComponent<Camera>();
    }

    void LateUpdate() {
        transform.rotation = CameraBrain.Instance.OutputCamera.transform.rotation;
        m_Camera.fieldOfView = CameraBrain.Instance.OutputCamera.fieldOfView;
    }
}
