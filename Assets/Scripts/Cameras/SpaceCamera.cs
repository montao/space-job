using UnityEngine;

public class SpaceCamera : MonoBehaviour {

    private Camera m_Camera;

    void Awake() {
        m_Camera = GetComponent<Camera>();
    }

    void LateUpdate() {
        Quaternion ship_rotation = default;
        if (ShipManager.Instance) {
            var angle = ShipManager.Instance.GetShipAngle();
            ship_rotation = Quaternion.Euler(0, angle, 0);
        }
        transform.rotation = ship_rotation * CameraBrain.Instance.OutputCamera.transform.rotation;
        m_Camera.fieldOfView = CameraBrain.Instance.OutputCamera.fieldOfView;
    }
}
