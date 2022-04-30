using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CockpitCam : NetworkBehaviour{

    private Camera cam;
    public GameObject cockpit;
    private Movnt moveScript;
    private StarParticles flying;
    private bool m_IsInArea = false;
    private NetworkVariable<bool> m_Cock = new NetworkVariable<bool>(true);
    void Start(){
        cam = GetComponent<Camera>();
        moveScript = cockpit.GetComponent<Movnt>();
        flying = cockpit.GetComponent<StarParticles>();
        
    }
    public override void OnNetworkSpawn(){
        m_Cock.OnValueChanged += OnStateChange;
    }
    public override void OnNetworkDespawn(){
        m_Cock.OnValueChanged -= OnStateChange;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SwitchServerRpc(){
        m_Cock.Value = !m_Cock.Value;
    }
    public void OnStateChange(bool previous, bool current){
        SetCamConditions(current);
    }

    void SetCamConditions(bool on){
        gameObject.SetActive(on);
    }
    private void Awake() {
        SetCamConditions(m_Cock.Value);
    }
    private void OnTriggerEnter(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            Debug.Log("Local player entered");
            m_IsInArea = true;
        }
        if(Input.GetKeyDown(KeyCode.C)){
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        PlayerAvatar player = other.GetComponent<PlayerAvatar>();
        if(player != null && player.IsOwner){
            Debug.Log("Local player exit");
            m_IsInArea = false;
            flying.setDrivingFalse();
        }
    }

    void Update(){
        if(m_IsInArea && Input.GetKeyDown(KeyCode.C)){
            Debug.Log("C pressed");
            cam.enabled = !cam.enabled;
            moveScript.enabled = true;
            flying.setDrivingTrue();
            SwitchServerRpc();
            
        }
/*         if(Input.GetKeyDown(KeyCode.X)){
            Debug.Log("x pressed");
            cam.enabled = !cam.enabled;
            moveScript.enabled = false;
            flying.setDrivingTrue();
        } */
    }
}
