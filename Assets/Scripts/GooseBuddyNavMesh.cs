using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooseBuddyNavMesh : MonoBehaviour
{
    private NavMeshAgent m_GooseNavMesh;
    public Transform Destination;
    void Start()
    {
        m_GooseNavMesh = GetComponent<NavMeshAgent>();
    }

    private void StealRandomItem(){

    }

    void Update()
    {

        m_GooseNavMesh.destination = Destination.position;
    }
}
