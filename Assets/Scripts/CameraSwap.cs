using UnityEngine;
using Cinemachine;

public class CameraSwap : MonoBehaviour {
    private CinemachineVirtualCamera m_Camera;
    public bool InRoom;

    void Start() {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        if(m_Camera == null){
            m_Camera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        if(!InRoom){
            LookAtPlayer();
        }
    }

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        if (player != null && player.IsOwner && CameraBrain.Instance.ActiveCameraObject != m_Camera.VirtualCameraGameObject) {
            if (InRoom){
                m_Camera.Priority = 30;
            }
            else{
                m_Camera.Priority = 20;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        var player = other.GetComponent<PlayerAvatar>();
        if (player != null && player.IsOwner) {
            m_Camera.Priority = 10;
        }
    }

    public void LookAtPlayer() {
        PlayerAvatar player = PlayerManager.Instance.LocalPlayer.Avatar;
        m_Camera.LookAt = player.transform;
    }
}
