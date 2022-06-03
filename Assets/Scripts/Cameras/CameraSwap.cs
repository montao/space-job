using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwap : MonoBehaviour {
    public CinemachineVirtualCamera m_Camera;
    public bool InRoom;

    public static bool CurrentSwapInRoom = false;
    private static List<CameraSwap> m_Instances = new List<CameraSwap>();

    void Start() {
        if(m_Camera == null){
            m_Camera = GetComponent<CinemachineVirtualCamera>();
        }
        if(m_Camera == null){
            m_Camera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        // I *think* this is not needed anymore because UpdateLookAt is
        // called whenever an avatar spawns -- not sure tho :)
        if(!InRoom) {
            if (PlayerManager.Instance.LocalPlayer != null) {
                PlayerAvatar avatar = PlayerManager.Instance.LocalPlayer.Avatar;
                if (avatar != null) {
                    LookAtPlayer(avatar);
                } else {
                    PlayerManager.Instance.LocalPlayer.OnAvatarChanged += LookAtPlayer;
                }
            }
        }

        if (!m_Camera) {
            Debug.LogWarning("No camera found for CameraSwap " + name);
        }

        m_Instances.Add(this);
    }

    public void LookAtPlayer(PlayerAvatar avatar) {
        m_Camera.LookAt = avatar.CameraLookAt.transform;
    }

    public void SwitchTo() {
        m_Camera.Priority = InRoom ? 30 : 20;
        PlayerManager.Instance.LocalPlayer.Avatar.HidePlayer(InRoom);
    }

    public void SwitchAway() {
        m_Camera.Priority = InRoom ? 5 : 10;
        if (InRoom) {
            PlayerManager.Instance.LocalPlayer.Avatar.HidePlayer(false);
        }
    }

    public static void UpdateLookAt(PlayerAvatar avatar) {
        foreach (var camera in m_Instances) {
            if (!camera.InRoom) {
                camera.LookAtPlayer(avatar);
            }
        }
    }
}
