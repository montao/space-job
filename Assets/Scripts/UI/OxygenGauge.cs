using UnityEngine;

[RequireComponent(typeof(Gauge))]
public class OxygenGauge : MonoBehaviour {
    [SerializeField]
    private Room m_Room;
    private Gauge m_Gauge;

    void Awake() {
        m_Gauge = GetComponent<Gauge>();
    }

    void Update() {
        m_Gauge.DisplayValue(m_Room.RoomOxygen);
    }
}
