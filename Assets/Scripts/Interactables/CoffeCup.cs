using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class CoffeCup : DroppableInteractable{
    private Int32 m_CupMaterialPattern = ~0x42CAFE43;
    private static int m_CupNumber = 0;
    public List<Material> Materials = new List<Material>();


// this would be always the same, so maybe it can be shorten?
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

    protected override void Interaction()
    {
        base.Interaction();
        SetServerRpc((int) NetworkManager.Singleton.LocalClientId);
    }
    
//----------------------------------------------------------------------------------------------

    public override int SelfInteraction(PlayerAvatar avatar) {
        avatar.SpeedBoost();
        return 6;
    }
}