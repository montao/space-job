using UnityEngine;
using UnityEngine.UI;

public class TerminalCanvas : MonoBehaviour {

    private GraphicRaycaster m_RayCaster;
    public Transform StuffKeeper;

    public void Awake() {
        m_RayCaster = GetComponent<GraphicRaycaster>();
        if (!m_RayCaster) {
            Debug.LogError("No GraphicRaycaster found for " + name + ".  Does the object have a canvas?");
        }
        SetInteractableEnable(false);
    }

    public void SetInteractableEnable(bool on) {
        m_RayCaster.enabled = on;
    }
}
