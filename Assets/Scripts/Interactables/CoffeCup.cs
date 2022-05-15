using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class CoffeCup : DroppableInteractable{
    private Int32 m_CupMaterialPattern = ~0x42CAFE43;
    private static int m_CupNumber = 0;
    public List<Material> Materials = new List<Material>();

//----------------------------------------------------------------------------------------------
    private void Awake() {
        m_AllCollider = new List<Collider>(GetComponentsInParent<Collider>());
        m_AllCollider.AddRange(GetComponents<Collider>());
        m_Mesh = GetComponentInParent<MeshRenderer>();
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_NetTransform = GetComponentInParent<NetworkTransform>();

        int mat_idx = (m_CupMaterialPattern >> (m_CupNumber++ % 32)) & 1;
        GetComponent<MeshRenderer>().material = Materials[mat_idx];
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(int value){
        pickedUp = true;
        m_State.Value = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DropServerRpc(Vector3 position) {
        GetComponentInParent<Rigidbody>().position = position;
        m_State.Value = IN_WORLD;
    }

    protected override void Interaction()
    {
        PlayerAvatar localPlayer = PlayerManager.Instance.LocalPlayer.Avatar;

        if (!localPlayer.HasInventorySpace()) {
            Debug.Log("Full of stuff");
            return;
        }

        m_IsInArea = false;
        localPlayer.AddToInventory(GetComponentInParent<NetworkObject>());

        SetServerRpc((int) NetworkManager.Singleton.LocalClientId);
    }
//----------------------------------------------------------------------------------------------

    public override int SelfInteraction(PlayerAvatar avatar) {
        avatar.SpeedBoost();
        return 6;
    }
}