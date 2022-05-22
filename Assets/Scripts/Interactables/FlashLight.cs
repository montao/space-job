using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class FlashLight : DroppableInteractable
{

    private Light m_Beam;
    // maybe needs to be network variable later
    private bool m_TurnedOn = true;
// this would be always the same, so maybe it can be shorten?
//----------------------------------------------------------------------------------------------
    private void Awake() {
        m_AllCollider = new List<Collider>(GetComponentsInParent<Collider>());
        m_AllCollider.AddRange(GetComponents<Collider>());
        m_Mesh = GetComponentInParent<MeshRenderer>();
        m_Rigidbody = GetComponentInParent<Rigidbody>();
        m_NetTransform = GetComponentInParent<NetworkTransform>();
        m_Beam = GetComponent<Light>();
    }

    protected override void Interaction()
    {
        base.Interaction();
        SetServerRpc((int) NetworkManager.Singleton.LocalClientId);
    }
    
//----------------------------------------------------------------------------------------------
    public override int SelfInteraction(PlayerAvatar avatar)
    {
        m_TurnedOn = !m_TurnedOn;
        m_Beam.enabled = m_TurnedOn;
        return 2;
    }
}
