using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private RectTransform m_HealthBar;
    private void Start() {
        m_HealthBar = GetComponent<RectTransform>();
    }

    public void UpdateHealthBar(float value){
        m_HealthBar.localScale = m_HealthBar.localScale * value;
    }
}
