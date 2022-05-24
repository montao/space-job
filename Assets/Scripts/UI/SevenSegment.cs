using System.Collections.Generic;
using UnityEngine;

public class SevenSegment : MonoBehaviour {

    [SerializeField]
    private List<Sprite> m_Sprites = new List<Sprite>();
    [SerializeField]
    private Sprite m_Minus;
    [SerializeField]
    private SpriteRenderer m_Renderer;
    [SerializeField]
    private SpriteRenderer m_DecimalRenderer;

    public void ShowDigit(int digit) {
        if (digit >= m_Sprites.Count) {
            return;
        }
        m_Renderer.sprite = m_Sprites[(int)digit];
    }

    public void ShowDecimal(bool show) {
        m_DecimalRenderer.enabled = show;
    }

    public void ShowMinus() {
        m_Renderer.sprite = m_Minus;
    }
}
