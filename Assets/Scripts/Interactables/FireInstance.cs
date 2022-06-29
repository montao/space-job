using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System;
using System.Collections.Generic;
public class FireInstance : RangedInteractableBase
{
    private float m_DamagePerTick;
    private Collider m_DeathField;
    private bool m_IsActive;
    public List<FireInstance> FireNeighbours;

    public override void Start() {
        base.Start();
        m_DeathField = GetComponent<Collider>();
    }



    protected override void Interaction(){

    }

    private void OnTriggerStay(Collider other) {
        var ava = other.gameObject.GetComponent<PlayerAvatar>();
        if (ava != null && ava.OwnerClientId == NetworkManager.Singleton.LocalClientId) {
            if (ava.m_Health.Value > 0) {
                ava.TakeDamage(0.01f);
            }
        }
    }
}
