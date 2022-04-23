using UnityEngine;
using Cinemachine;

public class CameraSwap : MonoBehaviour {
    private CinemachineVirtualCamera m_camera;

    void Start() {
        //m_camera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        //if (player != null && player.IsOwner && Camera.main != m_camera) {
            
        //}
    }
    private void OnTriggerExit(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        //if (player != null && player.IsOwner && Camera.main != m_camera) {
        //}
    }
}
