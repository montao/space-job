using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System;
using System.Collections.Generic;
public class FireInstance : RangedInteractableBase
{
    private float m_DamagePerTick;
    private DeathZone m_DeathField;
    private Transform m_SpawnLocation;
    private Room m_Room;
    private bool m_IsActive;
    public List<FireInstance> FireNeighbours;

    public override void Start() {
        base.Start();
        m_DeathField = GetComponentInChildren<DeathZone>();
    }

    protected override void Interaction(){
        if (PlayerAvatar.IsHolding<FireExtinguisher>()) {
            
        }
    }

    public void Setup(Room room, Transform spawn) {
        m_Room = room;
        name = "Fire (" + room.name + ")";
        m_SpawnLocation = spawn;
        //if (IsServer) {
        //    m_GrowCoroutine = StartCoroutine(Grow());
        //}
    }

    [ServerRpc(RequireOwnership=false)]
    public void ResolvedServerRpc() {
        Debug.Log("Fire extinguished in " + m_Room);
        m_Room.FireResolved(this, m_SpawnLocation);
        GetComponent<NetworkObject>().Despawn(destroy: true);  // breach do be gone
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
