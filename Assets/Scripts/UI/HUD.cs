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

    public TMP_Text primName;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        rotDir = new Vector3(0.0f, 1.0f, 0.0f);
        speed = 20.0f;
        camera.transform.position = new Vector3(0.0f, 0.0f, -2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        primObj.transform.Rotate(rotDir * Time.deltaTime * speed);
    }

    public void SetVisible(bool prim, bool vis) {
        if (prim) {
            primObj.SetActive(vis);
        }
        else {
            secObj.SetActive(vis);
        }
    }

    public void SetMesh(Mesh mesh, bool prim) {
        // not necessary but a bit nicer
        SetVisible(prim, false);
        speed = 0.0f;
        primObj.transform.Rotate(0.0f, 0.0f, 0.0f);
        secObj.transform.Rotate(0.0f, 0.0f, 0.0f);
        if (prim) {
            primObj.GetComponent<MeshFilter>().mesh = mesh;
        }
        else {
            secObj.GetComponent<MeshFilter>().mesh = mesh;
        }
        if (mesh != null) {
            float yscale = (prim) ? 0.4f : 0.25f;
            float fac = (1 / mesh.bounds.size.y) * yscale;
            if (prim) {
                float test = primObj.GetComponent<MeshRenderer>().bounds.min.y;
                Debug.Log("oh" + test);
                //float height = 0.1f - test.y;
                primObj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                primObj.transform.TransformPoint(0.0f, test, 0.0f);
                //Vector3 offset = primObj.transform.TransformPoint(mesh.bounds.center); 
                primObj.transform.localScale = new Vector3(fac, fac, fac);
                primObj.transform.position = new Vector3(0.2f, 0.0f, 0.0f);
                //primObj.transform.position = offset;       
            }
            else {
                secObj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                //Vector3 offset = secObj.transform.position - secObj.transform.TransformPoint(mesh.bounds.center); 
                secObj.transform.localScale = new Vector3(fac, fac, fac);
                secObj.transform.position = new Vector3(-0.3f, 0.15f, 0.0f);
            }      
        }

        SetVisible(prim, true);            
        speed = 20.0f;
    }

    public void SetName(string name) {
        primName.text = name;
    }

}
