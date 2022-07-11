using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;


public class HUD : NetworkBehaviour
{

    public static Mesh primMesh;
    public static Mesh secMesh;

    public MeshFilter primFilter;

    public MeshFilter secFilter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPMesh(GameObject primary) {
        if (primary != null) {
            primFilter = (MeshFilter)primary.GetComponent("MeshFilter");
            primMesh = primFilter.mesh;
        }
    }

    public void setSMesh(GameObject secondary) {
        if (secondary != null) {
            MeshFilter secFilter = (MeshFilter)secondary.GetComponent("MeshFilter");
            secMesh = secFilter.mesh;
        }
    }

}
