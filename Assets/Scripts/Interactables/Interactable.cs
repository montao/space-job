using UnityEngine;
using Unity.Netcode;

public abstract class Interactable<T> : NetworkBehaviour where T : unmanaged {

    public static Color HIGHLIGHT_COLOR = new Color(0.6f, 1.0f, 0.6f, 0.7f);

    protected bool m_IsInArea = false;
    protected NetworkVariable<T> m_State = new NetworkVariable<T>();
    public override void OnNetworkSpawn(){
        m_State.OnValueChanged += OnStateChange;
    }
    public override void OnNetworkDespawn(){
        m_State.OnValueChanged -= OnStateChange;
    }
    //will be called automatically for every client when new value is given.
    public abstract void OnStateChange(T previous, T current);
    private void OnTriggerEnter(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            m_IsInArea = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            m_IsInArea = false;
        }
    }
    protected abstract void Interaction();
    public T Value{
        get => m_State.Value;
    }

    void Start() {
    }


    public virtual void Update(){  
        if(m_IsInArea && Input.GetKeyDown(KeyCode.E)){
            Interaction();
        }
    }

    void OnWillRenderObject() {
        Shader.SetGlobalColor("_HighlightColor", m_IsInArea ? HIGHLIGHT_COLOR : Color.clear);
    }
}
