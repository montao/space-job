using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStars : MonoBehaviour
{
    public GameObject prefabStars;
    private GameObject currentStars;
    private GameObject newStars;

    void Start()
    {
        currentStars = Instantiate(prefabStars, transform.position, Quaternion.identity) as GameObject;
    }

    void OnTriggerExit(Collider other)
    {
        
        newStars = Instantiate(prefabStars, transform.position, Quaternion.identity) as GameObject;
        newStars.transform.parent = transform;
        currentStars = other.GetComponent<GameObject>();
        Destroy(currentStars);
        currentStars = newStars;  
        Debug.Log("hi");
    }
    void Update()
    {
        
    }
}
