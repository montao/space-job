using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class BreachBeGone : DroppableInteractable
{
// this would be always the same, so maybe it can be shorten?
//----------------------------------------------------------------------------------------------
    private void Awake() {
        m_AllCollider = new List<Collider>(GetComponentsInParent<Collider>());
        m_AllCollider.AddRange(GetComponents<Collider>());
        m_Mesh = GetComponentInParent<MeshRenderer>();
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_NetTransform = GetComponentInParent<NetworkTransform>();
    }

    protected override void Interaction()
    {
        base.Interaction();
        SetServerRpc((int) NetworkManager.Singleton.LocalClientId);
    }
    
//----------------------------------------------------------------------------------------------
    public override int SelfInteraction(PlayerAvatar avatar)
    {
        return 2;
    }
}
