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
    private float m_MaxSize = 1.5f;
    private Vector3 m_InitialSize;
    private Coroutine m_GrowCoroutine;
    public List<FireInstance> FireNeighbours;



    public override void Start() {
        base.Start();
        m_DeathField = GetComponentInChildren<DeathZone>();
        m_InitialSize = transform.localScale;
    }

    protected override void Interaction(){
        if (PlayerAvatar.IsHolding<FireExtinguisher>()) {
            ResolvedServerRpc();
        }
    }

    public void Setup(Room room, Transform spawn) {
        m_Room = room;
        name = "Fire (" + room.name + ")";
        m_SpawnLocation = spawn;
        if (IsServer) {
            m_GrowCoroutine = StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow() {
        while(transform.localScale.x < m_MaxSize){
            yield return new WaitForSeconds(1f);
            GrowFireClientRpc();
        }
    }

    [ServerRpc(RequireOwnership=false)]
    public void ResolvedServerRpc() {
        Debug.Log("Fire extinguished in " + m_Room);
        m_Room.FireResolved(this, m_SpawnLocation);
        GetComponent<NetworkObject>().Despawn(destroy: true);  // breach do be gone
    }

    [ClientRpc]
    public void GrowFireClientRpc() {
        transform.localScale = 1.1f * transform.localScale;
    }
}
