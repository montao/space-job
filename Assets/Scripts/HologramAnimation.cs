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
        if (m_HoloAnimator != null) {
            m_HoloAnimator.SetInteger("active", current);
        }
    }

    private void Start() {
        if (IsServer) {
            m_Animation.Value = ActiveAnimation;
        }
        m_HoloAnimator = GetComponent<Animator>();
        if (!m_HoloAnimator) {
            Debug.LogWarning("No Animator component found on Hologram " + gameObject.name);
        }
    }
}
