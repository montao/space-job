using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public abstract class DroppableInteractable : Interactable<int>{
    protected MeshRenderer m_Mesh;
    protected Rigidbody m_Rigidbody;
    protected List<Collider> m_AllCollider;
    protected NetworkTransform m_NetTransform;
    public const int IN_WORLD = -1;
    public float velocity;

//----------------------------------------------------------------------------------------------
    
    public virtual void Awake() {
        m_AllCollider = new List<Collider>(GetComponentsInParent<Collider>());
        m_AllCollider.AddRange(GetComponents<Collider>());
        m_Mesh = GetComponentInParent<MeshRenderer>();
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_NetTransform = GetComponentInParent<NetworkTransform>();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsServer) {
            m_State.Value = IN_WORLD;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void SetHolderServerRpc(int holder_id){
        m_State.Value = holder_id;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DropServerRpc(Vector3 position) {
        GetComponentInParent<Rigidbody>().position = position;
        m_State.Value = IN_WORLD;
    }


    protected override void Interaction(){
        PlayerAvatar localPlayer = PlayerManager.Instance.LocalPlayer.Avatar;

        if (!localPlayer.HasInventorySpace()) {
            Debug.Log("Full of stuff");
            return;
        }

        m_IsInArea = false;
        localPlayer.AddToInventory(GetComponentInParent<NetworkObject>());
        SetHolderServerRpc((int) NetworkManager.Singleton.LocalClientId);
    }

    public override void OnStateChange(int previous, int current) {
        if (current != previous) {
            // inWorld changed, i.e. item was dropped or picked up
            UpdateWorldstate(current == IN_WORLD);
        }
    }


    public void UpdateWorldstate(bool inWorld) {
        foreach(Collider colli in m_AllCollider){
            colli.enabled = inWorld;
        }
        if (inWorld) {
            // Disabling the renderer after picking the item up is done by the
            // playeravatar, because it needs access to the renderer to steal
            // its mesh and materials
            m_Mesh.enabled = true;
        }
        m_Rigidbody.isKinematic = !inWorld; // TODO does this work? 
        m_NetTransform.enabled = inWorld;
    }
}
