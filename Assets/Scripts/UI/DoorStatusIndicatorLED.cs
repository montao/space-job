using UnityEngine;

public class DoorStatusIndicatorLED : MonoBehaviour {
    [SerializeField]
    private Door m_Door;
    [SerializeField]
    private Material m_MaterialOn;
    [SerializeField]
    private Material m_MaterialOff;

    private Renderer m_Rend;

    void Awake() {
        m_Rend = GetComponent<Renderer>();
    }

    void Start() {
        m_Door.OnDoorOpen += () => {
            m_Rend.material = m_MaterialOn;
        };

        m_Door.OnDoorClose += () => {
            m_Rend.material = m_MaterialOff;
        };
    }
}
