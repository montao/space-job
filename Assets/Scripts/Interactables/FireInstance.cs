using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System;
using System.Collections.Generic;
public class FireInstance : RangedInteractableBase
{
    private float m_DamagePerTick;
    private DeathZone m_DeathField;
    private bool m_IsActive;
    public List<FireInstance> FireNeighbours;

    public override void Start() {
        base.Start();
        m_DeathField = GetComponentInChildren<DeathZone>();
    }

    public void SetActive(bool new_state){
        
    }

    protected override void Interaction(){
        if (PlayerAvatar.IsHolding<FireExtinguisher>()) {
            transform.localScale = transform.localScale * 100f;
        }
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
