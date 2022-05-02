using System.Collections;
using System.Collections.Generic;
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


    

    protected override void Interaction(){
        SetServerRpc(!Value);
        SetCockConditions(!Value);
    }

    void SetCockConditions(bool on){
        GameObject stars = GameObject.FindGameObjectWithTag("Stars");
        star_bool = stars.GetComponent<StarParticles>();
        //if(IsLocalPlayer){
            cam.Priority = cam_prio;
            PlayerManager.Instance.LocalPlayer.Avatar.GetComponent<CharacterController>().enabled = !on;
            if(!on){
            cam.Priority = 0;
            //}
        }
        
        star_bool.driving = on;
        cam_front.GetComponent<Movnt>().enabled = on;
        cam_left.GetComponent<Movnt>().enabled = on;
        cam_right.GetComponent<Movnt>().enabled = on;
        
        
    }
    public override void OnStateChange(bool previous, bool current){
        SetCockConditions(previous);
    }
    [ServerRpc(RequireOwnership = false)]

    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

}
