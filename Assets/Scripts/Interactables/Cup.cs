using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/**
  * Mjam, that's some good coffee~

  Feckn delicious shit, oh no..
  */

public class Cup : Interactable<bool>{
    private MeshRenderer m_Mesh;
    private Rigidbody m_Rigidbody;
    private List<Collider> m_AllCollider;

    protected override void Interaction(){
        SetServerRpc(!Value);
    }

    public override void OnStateChange(bool previous, bool current){
        if (current != previous) {
            // inWorld changed, i.e. item was dropped or
            // picked up
            UpdateWorldstate(current);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

    private void Awake() {
        m_AllCollider = new List<Collider>(GetComponents<Collider>());
        m_Mesh = GetComponent<MeshRenderer>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void UpdateWorldstate(bool inWorld){
        foreach(Collider colli in m_AllCollider){
            colli.enabled = inWorld;
        }
        m_Mesh.enabled = inWorld;
        m_Rigidbody.isKinematic = !inWorld && !IsOwner; // TODO does this work? 
    }
}
