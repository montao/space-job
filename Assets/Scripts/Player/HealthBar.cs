using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private RectTransform m_HealthBar;
    private void Start() {
        m_HealthBar = GetComponent<RectTransform>();
    }

    public void UpdateHealthBar(float health){
        m_HealthBar.localScale = new Vector3(health*40 ,m_HealthBar.localScale.y ,m_HealthBar.localScale.z );
    }
}
