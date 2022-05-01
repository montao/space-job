using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTravel : MonoBehaviour
{
    public float speed = 0.1f;
    //private float maxSpeed = 100.0f;

    private Camera cam;

    private float timeCount = 0.0f;

    void Start() {
        cam = Camera.main;
    }


    void Update() {
        if(Input.GetKey(KeyCode.W)){
/*          transform.Rotate(cam.transform.forward, timeCount);
  */         Quaternion.LookRotation(cam.transform.forward, transform.forward);  
     }
        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.up, timeCount);
        }
        if(Input.GetKey(KeyCode.D)){
               
        }
        if(Input.GetKey(KeyCode.S)){
            
        }  
        timeCount = timeCount + Time.deltaTime;

    }
}
