using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;


public class HUD : NetworkBehaviour
{


    public GameObject primObj, secObj;

    private Vector3 rotDir;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        rotDir = new Vector3(0.0f, 1.0f, 0.0f);
        speed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        primObj.transform.Rotate(rotDir * Time.deltaTime * speed);
    }

    public void setMesh(Mesh mesh, bool prim) {
        if (prim) {
            primObj.GetComponent<MeshFilter>().mesh = mesh;
        }
        else {
            secObj.GetComponent<MeshFilter>().mesh = mesh;
        }
    }

}
