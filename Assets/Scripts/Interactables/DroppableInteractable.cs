using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public abstract class DroppableInteractable : Interactable<int>{
    protected MeshRenderer m_Mesh;
    protected Rigidbody m_Rigidbody;
    protected List<Collider> m_AllCollider;
    protected NetworkTransform m_NetTransform;
    public float Velocity = 100;
    public const int IN_WORLD = -1;
    [SerializeField]
    protected AudioClip[] pickupSounds;
    [SerializeField]
    protected AudioClip[] interactionSounds;
    protected AudioSource audioSource;

    public delegate void OnPickupDropDelegate(PlayerAvatar avatar);
    public event OnPickupDropDelegate OnPickup;
    public event OnPickupDropDelegate OnDrop;

    public Vector3 PrimaryPos;
    public Vector3 PrimaryRot;
    public Vector3 SecondaryPos;
    public Vector3 SecondaryRot;

//----------------------------------------------------------------------------------------------
    
    public virtual void Awake() {
        m_AllCollider = new List<Collider>(GetComponentsInParent<Collider>());
        m_AllCollider.AddRange(GetComponents<Collider>());
        m_Mesh = GetComponentInParent<MeshRenderer>();
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_NetTransform = GetComponentInParent<NetworkTransform>();
        audioSource = GetComponent<AudioSource>(); 
    }
    protected AudioClip GetRandomInteractionClip(){
        return interactionSounds[UnityEngine.Random.Range(0, interactionSounds.Length)];
    }
    protected AudioClip GetRandomPickUpClip(){
        return pickupSounds[UnityEngine.Random.Range(0, pickupSounds.Length)];
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
        m_Rigidbody.position = position;
        m_State.Value = IN_WORLD;
        m_Rigidbody.AddForce(Vector3.Normalize(PlayerManager.Instance.LocalPlayer.Avatar.transform.forward) * Velocity);

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
        AudioClip sound = GetRandomPickUpClip();
        audioSource.PlayOneShot(sound);
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

    public void InvokeOnPickup(PlayerAvatar avatar) {
        OnPickup?.Invoke(avatar);
    }

    public void InvokeOnDrop(PlayerAvatar avatar) {
        OnDrop?.Invoke(avatar);
    }
}
