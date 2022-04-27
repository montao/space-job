using UnityEngine;

public class InteractionRange : MonoBehaviour {

    public delegate void OnRangeTriggerEventHandler(Collider other);
    public event OnRangeTriggerEventHandler OnRangeTriggerEnter;
    public event OnRangeTriggerEventHandler OnRangeTriggerExit;

    protected void OnTriggerEnter(Collider other) {
        Debug.Log("boop " + transform.parent.gameObject.name);
        OnRangeTriggerEnter(other);
    }

    protected void OnTriggerExit(Collider other) {
        OnRangeTriggerExit(other);
    }
}
