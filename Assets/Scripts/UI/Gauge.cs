using UnityEngine;

public class Gauge : MonoBehaviour {
    public float MinAngle;
    public float MaxAngle;
    public Transform Needle;

    public void DisplayValue(float val) {
        float angle = Mathf.Lerp(MinAngle, MaxAngle, val);
        Needle.localRotation = Quaternion.Euler(0, angle, 0);
    }

}
