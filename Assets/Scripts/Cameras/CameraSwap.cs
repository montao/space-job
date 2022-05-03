using Cinemachine;
using UnityEngine;

public class CameraSwap : MonoBehaviour {
    public CinemachineVirtualCamera m_Camera;
    public bool InRoom;

    void Start() {
        if(m_Camera == null){
            m_Camera = GetComponent<CinemachineVirtualCamera>();
        }
        if(m_Camera == null){
            m_Camera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        if(!InRoom) {
            PlayerAvatar avatar = PlayerManager.Instance.LocalPlayer.Avatar;
            if (avatar != null) {
                LookAtPlayer(avatar);
            } else {
                PlayerManager.Instance.LocalPlayer.OnAvatarChanged += LookAtPlayer;
            }
        }

        if (!m_Camera) {
            Debug.LogWarning("No camera found for CameraSwap " + name);
        }
    }

    public void LookAtPlayer(PlayerAvatar avatar) {
        m_Camera.LookAt = avatar.transform;
    }

    public void SwitchTo() {
        m_Camera.Priority = InRoom ? 30 : 20;
    }

    public void SwitchAway() {
        m_Camera.Priority = InRoom ? 5 : 10;
    }
}
