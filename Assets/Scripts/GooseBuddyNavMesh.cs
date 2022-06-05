using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GooseBuddyNavMesh : MonoBehaviour
{
    private NavMeshAgent m_GooseNavMesh;
    public Transform Destination;
    // Start is called before the first frame update
    void Start()
    {
        m_GooseNavMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        m_GooseNavMesh.destination = Destination.position;
    }
}
