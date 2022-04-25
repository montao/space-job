using UnityEngine;
using Unity.Netcode;

public abstract class Interactable<T> : NetworkBehaviour where T : unmanaged {

    private LayerMask _highlightedLayer;
    private LayerMask _defaultLayer;

    private MeshRenderer _renderer;

    void Start() {
        _highlightedLayer = LayerMask.NameToLayer("Highlighted");
        _renderer = GetComponent<MeshRenderer>();
        if (!_renderer) {
            _renderer = GetComponentInChildren<MeshRenderer>();
        }
        if (!_renderer) {
            _renderer = GetComponentInParent<MeshRenderer>();
        }

        if (_renderer) {
            _defaultLayer = _renderer.gameObject.layer;
        } else {
            Debug.LogWarning("No renderer found for " + gameObject.name);
        }
    }

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
            _renderer.gameObject.layer = _highlightedLayer;
        }
    }
    private void OnTriggerExit(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            m_IsInArea = false;
            _renderer.gameObject.layer = _defaultLayer;
        }
    }
    protected abstract void Interaction();
    public T Value{
        get => m_State.Value;
    }


    public virtual void Update(){  
        if(m_IsInArea && Input.GetKeyDown(KeyCode.E)){
            Interaction();
        }
    }
}
