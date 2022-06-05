using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooseBuddyNavMesh : MonoBehaviour
{
    private NavMeshAgent m_GooseNavMesh;
    private Collider m_FollowingRange;
    public Transform Destination;
    void Start()
    {
        m_GooseNavMesh = GetComponent<NavMeshAgent>();
        m_FollowingRange = GetComponent<Collider>();
    }

    private void SetEvilBehaviour(){

    }

    private void SetBuddyBehaviour(){

    }

    // idle through ship
    // follow player
    // steal mission (priorieat)

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<PlayerAvatar>() != null){
            Destination = other.gameObject.transform;
        }
    }

    void Update()
    {
        m_GooseNavMesh.destination = Destination.position;
    }
}
