using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Samples;

[RequireComponent(typeof(ClientNetworkTransform))]
public class Movnt : NetworkBehaviour {
    private Vector3 lastMousePos = new Vector3(255, 255, 255);
    float sensitivity = 0.25f;

    

    void Update(){

        lastMousePos = Input.mousePosition - lastMousePos;
        lastMousePos *= sensitivity;
        lastMousePos = new Vector3(transform.eulerAngles.x - lastMousePos.y, transform.eulerAngles.y + lastMousePos.x, 0); // x-rotation + mouse.y
        transform.eulerAngles = lastMousePos;
        lastMousePos = Input.mousePosition;

    }
}
