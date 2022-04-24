using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/**
  * Mjam, that's some good coffee~

  Feckn delicious shit, oh no..
  */

public class Cup : Interactable<int>{
    private MeshRenderer m_Mesh;
    private Rigidbody m_Rigidbody;
    private List<Collider> m_AllCollider;
    public const int IN_WORLD = -1;

    public void Start() {
        m_State.Value = IN_WORLD;
    }

    protected override void Interaction() {
        PlayerAvatar localPlayer = PlayerManager.Instance.LocalPlayer.Avatar;
        Debug.Log("glug " + localPlayer);
        SetServerRpc((int) NetworkManager.Singleton.LocalClientId); // weardes casting thing
    }

    public override void OnStateChange(int previous, int current){
        if (current != previous) {
            // inWorld changed, i.e. item was dropped or
            // picked up
            UpdateWorldstate(current == IN_WORLD);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(int value){
        m_State.Value = value;
    }

    private void Awake() {
        m_AllCollider = new List<Collider>(GetComponentsInParent<Collider>());
        m_AllCollider.AddRange(GetComponents<Collider>());
        m_Mesh = GetComponentInParent<MeshRenderer>();
        m_Rigidbody = GetComponentInParent<Rigidbody>();
    }

    public void UpdateWorldstate(bool inWorld){
        foreach(Collider colli in m_AllCollider){
            colli.enabled = inWorld;
        }
        m_Mesh.enabled = inWorld;
        m_Rigidbody.isKinematic = !inWorld; // TODO does this work? 
    }
}
