using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    [Range(0f,100f)]
    private float m_Health = 100f;
    private RectTransform m_HealthBar;
}
