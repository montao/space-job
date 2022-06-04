using UnityEngine;
using Unity.Netcode;

public enum PlayerAnimation {
    INTERACT,
    ARMWAVE,
    SIT,
    JUMP,
    DRINK,
}

public class PlayerAnimationController : NetworkBehaviour {
    private Animator m_PlayerAnimator;
    private PlayerAvatar m_Player;

    private void Awake() {
        m_PlayerAnimator = GetComponent<Animator>();
        m_Player = GetComponent<PlayerAvatar>();
    }

    [ClientRpc]
    public void TriggerAnimationClientRpc(PlayerAnimation animation){
        m_PlayerAnimator.SetTrigger(animation.ToString().ToLower());
    }
    
    public void OnSpeedChange(float prev, float current){
        m_PlayerAnimator.SetFloat("speed", current);
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        m_Player.HorizontalSpeed.OnValueChanged += OnSpeedChange;
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();
        m_Player.HorizontalSpeed.OnValueChanged -= OnSpeedChange;

    }
}
