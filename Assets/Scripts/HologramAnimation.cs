using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HologramAnimation : NetworkBehaviour
{
    public int ActiveAnimation = 0;
    private NetworkVariable<int> m_Animation = new NetworkVariable<int>(0); 
    private Animator m_HoloAnimator;
    
    public override void OnNetworkSpawn(){
        m_Animation.OnValueChanged += OnStateChange;
    }
    public override void OnNetworkDespawn(){
        m_Animation.OnValueChanged -= OnStateChange;
    }
    public void OnStateChange(int previous, int current){
        m_HoloAnimator.SetInteger("active", current);
    }

    private void Start() {
        m_HoloAnimator = GetComponent<Animator>();
    }
    private void Update() {
        m_Animation.Value = ActiveAnimation;
    }
}
