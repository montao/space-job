using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private float m_DamagePerTick;
    private Collider DeathField;
    private void Start() {
        DeathField = GetComponent<Collider>();
    }
    private void OnTriggerStay(Collider other) {
        Debug.Log("hi");
        if (other.gameObject.GetComponent<PlayerAvatar>() != null){
            other.gameObject.GetComponent<PlayerAvatar>().TakeDamage(0.01f);
        } 
    }
}
