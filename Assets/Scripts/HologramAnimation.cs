using UnityEngine;
using Unity.Netcode;

public class HologramAnimation : NetworkBehaviour
{
    public int ActiveAnimation;
    private NetworkVariable<int> m_Animation = new NetworkVariable<int>(-1); 
    private Animator m_HoloAnimator;
    
    public override void OnNetworkSpawn(){
        m_Animation.OnValueChanged += OnStateChange;
        m_Animation.Value = ActiveAnimation;
    }
    public override void OnNetworkDespawn(){
        m_Animation.OnValueChanged -= OnStateChange;
    }
    public void OnStateChange(int previous, int current){
        if (m_HoloAnimator != null) {
            Debug.Log("old State:" + current);
            Debug.Log("new State:" + current);
            //m_HoloAnimator.SetInteger("active", current);
        }
    }

    private void Update() {
        m_HoloAnimator.SetInteger("active", m_Animation.Value);
    }

    private void Start() {
        m_HoloAnimator = GetComponent<Animator>();
        if (!m_HoloAnimator) {
            Debug.LogWarning("No Animator component found on Hologram " + gameObject.name);
        }
    }
}
