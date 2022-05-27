using System.Collections.Generic;
using UnityEngine;

public class SevenSegmentDisplay : MonoBehaviour {
    [SerializeField]
    private List<SevenSegment> m_Segments = new List<SevenSegment>();

    public void DisplayNumber(float number, int decimals) {
        int n = (int) Mathf.Abs(Mathf.Round(number * Mathf.Pow(10, decimals)));
        for (int i = m_Segments.Count - 1; i >= 0; --i) {
            m_Segments[i].ShowDigit(n % 10);
            m_Segments[i].ShowDecimal(i == m_Segments.Count - 1 - decimals);
            n /= 10;
        }
        if (number < 0) {
            m_Segments[0].ShowMinus();
        }
    }
}
