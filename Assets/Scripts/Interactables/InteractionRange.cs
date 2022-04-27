using UnityEngine;

public class InteractionRange : MonoBehaviour {

    public delegate void OnRangeTriggerEventHandler(Collider other);
    public event OnRangeTriggerEventHandler OnRangeTriggerEnter;
    public event OnRangeTriggerEventHandler OnRangeTriggerExit;

    void Start() {
        if (gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")) {
            Debug.LogWarning("InteractionRange not on Ignore Raycast layer :(" + gameObject.name);
        }
    }

    protected void OnTriggerEnter(Collider other) {
        OnRangeTriggerEnter(other);
    }

    protected void OnTriggerExit(Collider other) {
        OnRangeTriggerExit(other);
    }
}
