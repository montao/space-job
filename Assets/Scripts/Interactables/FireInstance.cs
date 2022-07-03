using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System;
using System.Collections.Generic;
using Random=UnityEngine.Random;


public class FireInstance : RangedInteractableBase
{
    private float m_DamagePerTick;
    private DeathZone m_DeathField;
    private Transform m_SpawnLocation;
    private Room m_Room;
    private bool m_IsActive;
    private float m_MaxSize = 1.5f;
    private bool m_CanJump = false;
    private Vector3 m_InitialSize;
    private Coroutine m_GrowCoroutine;
    private Coroutine m_JumpCoroutine;
    public List<FireInstanceSpawnLocation> NeighbourFireLocation;
    private List<FireInstance> m_NeighbourFires;



    public override void Start() {
        base.Start();
        m_DeathField = GetComponentInChildren<DeathZone>();
        m_InitialSize = transform.localScale;
        m_DeathField.SetDamage(0.005f);
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
            m_JumpCoroutine = StartCoroutine(Jump());
        }
    }

    private IEnumerator Grow() {
        while(transform.localScale.x < m_MaxSize){
            yield return new WaitForSeconds(1f + Random.Range(-0.5f, 0.5f));
            GrowFireClientRpc();
            if(transform.localScale.x >= 1f){
                m_CanJump = true;
            }
        }
        //Debug.Log("Grown to compleation");
    }
    private IEnumerator Jump() {
        while(!m_CanJump){
            yield return new WaitForSeconds(1f);
        }
        while(m_CanJump){
            yield return new WaitForSeconds(5f + Random.Range(0f, 30f));

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
