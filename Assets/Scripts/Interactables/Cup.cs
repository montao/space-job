using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/**
  * Mjam, that's some good coffee~

  Feckn delicious shit, oh no..
  */

[System.Serializable]
public struct Itemstate{
    public Vector3 location;
    public Quaternion rotation;
    public bool inWorld;
}
public class Cup : Interactable<Itemstate>{
    private MeshRenderer m_Mesh;
    private Rigidbody m_Rigidbody;
    private List<Collider> m_AllCollider;
    protected override void Interaction(){
        Itemstate newState = Value;
        newState.inWorld = !newState.inWorld;
        SetServerRpc(newState);
    }
    public override void OnStateChange(Itemstate previous, Itemstate current){
        if (current.inWorld != previous.inWorld) {
            // inWorld changed, i.e. item was dropped or
            // picked up
            UpdateWorldstate(current.inWorld);
        }
        if (current.inWorld && !IsOwner) {
            transform.position = current.location;
            transform.rotation = current.rotation;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(Itemstate value){
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

    public override void Update() {
        base.Update();
        if (IsOwner && Value.inWorld) {
            Itemstate newState = Value;
            newState.location = transform.position;
            newState.rotation = transform.rotation;
            SetServerRpc(Value);
        }
    }
}
