using UnityEngine;

public class InteractableExitCanvas : MonoBehaviour {

    private Canvas m_Canvas;

    public static InteractableExitCanvas Instance;

    void Awake() {
        m_Canvas = GetComponent<Canvas>();
        Instance = this;
    }

    public void SetVisible(bool visible) {
        m_Canvas.enabled = visible;
    }

    public void OnButtonPress() {
        TwoLevelInteractable.TryExitLocalPlayerFromActive();
    }
}
