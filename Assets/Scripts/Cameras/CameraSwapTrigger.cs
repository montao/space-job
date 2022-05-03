using UnityEngine;

public class CameraSwapTrigger : CameraSwap {

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        if (player != null && player.IsOwner && CameraBrain.Instance.ActiveCameraObject != m_Camera.VirtualCameraGameObject) {
            SwitchTo();
        }
    }
    private void OnTriggerExit(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        if (player != null && player.IsOwner) {
            SwitchAway();
        }
    }
}
