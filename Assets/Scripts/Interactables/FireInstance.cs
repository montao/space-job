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
    private float m_MaxSize = 1f;
    private bool m_CanJump = false;
    private Vector3 m_InitialSize;
    private Coroutine m_GrowCoroutine;
    private Coroutine m_JumpCoroutine;
    //public List<FireInstanceSpawnLocation> NeighbourFireLocations;
    private List<Transform> m_NeighbourTranforms;
    public List<Transform> NeighbourTransforms{
        get => m_NeighbourTranforms;
        set => m_NeighbourTranforms = value;
    } 
    private List<FireInstance> m_BurningNeighbours;
    public List<FireInstance> BurningNeighbours{
        get => m_BurningNeighbours;
        set => m_BurningNeighbours = value;
    }     
    public List<FireInstanceSpawnLocation> NeighbourSpawnLocations;
    public Room GetRoom(){
        return m_Room;
    }

    public override void Start() {
        base.Start();
        m_DeathField = GetComponentInChildren<DeathZone>();
        m_InitialSize = transform.localScale;
        m_DeathField.SetDamage(0.005f);
        foreach(FireInstanceSpawnLocation neighbour in NeighbourSpawnLocations){
            m_NeighbourTranforms.Add(neighbour.transform);
        } 
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
            yield return new WaitForSeconds(2f + Random.Range(-0.5f, 0.5f));
            GrowFireClientRpc();
            if(transform.localScale.x >= 0.8f){
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
            //yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(10f + Random.Range(0f, 30f));
            m_Room.SpawnFire();
            //m_Room.FireJumpOver(this);
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
