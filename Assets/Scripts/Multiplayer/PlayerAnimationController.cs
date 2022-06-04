using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public static class Animation{
    public const string INTERACT = "interact";
    public const string ARMWAVE = "armwave";
    public const string SIT = "sit";
    public const string JUMP = "jump";
    public const string DRINK = "drink";
}

public class PlayerAnimationController : NetworkBehaviour {
    private Animator m_PlayerAnimator;
    private PlayerAvatar m_Player;

    private void Awake() {
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player = GetComponent<PlayerAvatar>();
    }

    [ClientRpc]
    public void TriggerAnimationClientRpc(string animation){
        m_PlayerAnimator.SetTrigger(animation);
    }
    
    public void OnSpeedChange(float prev, float current){
        m_PlayerAnimator.SetFloat("speed", current);
    }

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        m_Player.HorizontalSpeed.OnValueChanged += OnSpeedChange;
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }
}