using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
    private Button[] m_Buttons;
    private Canvas m_Canvas;

    void Start() {
        m_Canvas = GetComponent<Canvas>();
        m_Buttons = GetComponentsInChildren<Button>(includeInactive: true);
    }

    public void OnButtonPress() {
        ShipManager.Instance.StartNewGameServerRpc();
    }

    public void SetEnabled(bool enabled) {
        if (m_Buttons != null) {
            foreach (var button in m_Buttons) {
                if (button) {
                    button.gameObject.SetActive(enabled);
                }
            }
        }
        if (m_Canvas != null) {
            m_Canvas.enabled = enabled;
        }
    }
}
