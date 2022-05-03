using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class Cockpit : Interactable<bool> {

    public CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera cam_front;
    public CinemachineVirtualCamera cam_left;
    public CinemachineVirtualCamera cam_right;
    public int cam_prio;
    private StarParticles star_bool;

    private bool m_LocalPlayerInteracting = false;

    // Called only for the player that triggered the interaction
    protected override void Interaction() {
        if (!m_LocalPlayerInteracting && Value) {
            // cockpit occupied
            return;
        }
        m_LocalPlayerInteracting = !m_LocalPlayerInteracting;
        SetServerRpc(m_LocalPlayerInteracting);
        SetCockConditions(m_LocalPlayerInteracting);
    }

    void SetCockConditions(bool on){
        GameObject stars = GameObject.FindGameObjectWithTag("Stars");
        star_bool = stars.GetComponent<StarParticles>();
        cam.Priority = cam_prio;
        /* player = PlayerManager.Instance.LocalPlayer.Avatar; */
        /* foreach (var player in FindObjectsOfType<PersistentPlayer>()) { */
            //Debug.Log(player.Avatar.name);
        PlayerManager.Instance.LocalPlayer.Avatar.GetComponent<CharacterController>().enabled = !on;
        star_bool.driving = on;
        cam_front.GetComponent<Movnt>().enabled = on;
        cam_left.GetComponent<Movnt>().enabled = on;
        cam_right.GetComponent<Movnt>().enabled = on;
        if(!on){
            cam.Priority = 0;
        }
        
    }

    // Called for all players when the state value changes
    public override void OnStateChange(bool previous, bool current){
        //SetCockConditions(previous);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

}
