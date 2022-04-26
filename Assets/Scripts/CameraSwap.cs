using UnityEngine;
using Cinemachine;

public class CameraSwap : MonoBehaviour {
    private CinemachineVirtualCamera m_Camera;

    void Start() {
        m_Camera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();

        if (player != null && player.IsOwner && CameraBrain.Instance.ActiveCameraObject != m_Camera.VirtualCameraGameObject) {
            m_Camera.Priority = 20;
            m_Camera.LookAt = player.transform;
        }
    }
    private void OnTriggerExit(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        if (player != null && player.IsOwner && CameraBrain.Instance.ActiveCameraObject != m_Camera.VirtualCameraGameObject) {
            m_Camera.Priority = 10;
        }
    }
}
