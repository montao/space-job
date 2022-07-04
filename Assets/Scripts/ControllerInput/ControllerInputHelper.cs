using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControllerInputHelper : MonoBehaviour {
    [SerializeField]
    private TMP_Text m_DebugText;

    private List<InteractableBase> m_AvailableInteractables = new List<InteractableBase>();

    public void MarkInteractableAvailable(InteractableBase interactable) {
        m_AvailableInteractables.Add(interactable);
    }

    public bool MarkInteractableUnavailable(InteractableBase interactable) {
        return m_AvailableInteractables.Remove(interactable);
    }

    void Update() {
        string text = "AVAILABLE INTERACTABLES:\n";
        foreach (var i in m_AvailableInteractables) {
            text += i.name + "\n";
        }
        m_DebugText.text = text;
    }
}
