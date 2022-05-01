using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapControl : MonoBehaviour {
    public bool outOfMap = false;

    void OnTriggerExit(Collider other) {
        Debug.Log("Ship too far off!");
        if(other.tag == "MapShip"){
            outOfMap = true;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
