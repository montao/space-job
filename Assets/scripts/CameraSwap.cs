using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    private Camera activeCamera;
    public Camera nextCamera;
    private Collider activeArea;

    void Start()
    {
        activeArea = GetComponentInChildren<Collider>();
        activeCamera = GetComponent<Camera>();
    }

    private void OnTriggerExit(Collider other) {
        if(other.GetComponent<CharacterController>() != null)
            activeCamera.enabled = false;
            nextCamera.enabled = true;
    }
}
