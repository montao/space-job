using UnityEngine;

/**
 * Rigidbodies swallow events, including OnMouseOver and OnMouseExit.
 *
 * For interactables where there's a Rigidbody parented to the actual
 * Interactable object (i.e. the coffee cup), place this component
 * onto the parent object to make interaction work.
 */
public class FixMouseOverNotPassingToChildren : MonoBehaviour {
    private Transform[] m_Children;

    void Start() {
        m_Children = GetComponentsInChildren<Transform>();
    }

    private void BroadcastToChildren(string message) {
        foreach (var child in m_Children) {
            if (child.transform != this.transform) {
                child.BroadcastMessage(message, options: SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnMouseOver() {
        BroadcastToChildren("OnMouseOver");
    }

    void OnMouseExit() {
        BroadcastToChildren("OnMouseExit");
    }
}
