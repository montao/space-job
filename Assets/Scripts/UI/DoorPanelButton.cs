using UnityEngine;

public class DoorPanelButton : RangedInteractableBase {
    public enum Type { OPEN, CLOSE }
    [SerializeField]
    private Type m_Type;
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

    public override void Start() {
        base.Start();

        m_Door.OnDoorOpen += () => {
            m_Rend.material = (m_Type == Type.OPEN) ? m_MaterialOn : m_MaterialOff;
        };

        m_Door.OnDoorClose += () => {
            m_Rend.material = (m_Type == Type.CLOSE) ? m_MaterialOn : m_MaterialOff;
        };
    }

    protected override void Interaction() {
        bool open = m_Type == Type.OPEN;
        m_Door.SetDoorStatusServerRpc(open);
    }
}
