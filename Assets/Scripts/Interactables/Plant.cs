using UnityEngine;
using Unity.Netcode;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

public class Plant : Interactable<bool> {
    [SerializeField]
    private AudioClip plantSound;
    private AudioSource audioSource;

    private Mesh plantPot;
    [SerializeField]
    private Mesh seedInPot;
    [SerializeField]
    private Mesh plantStage1;
    [SerializeField]
    private Mesh deadPlant;
    [SerializeField]
    private Mesh dryPlant;
    private Mesh healthyPlant;
    private MeshFilter currentMesh;
    private NetworkObject seed;
    public NetworkVariable<int> m_mesh = new NetworkVariable<int>(1);
    public NetworkVariable<bool> notPlanted = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> grown = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> seedPlanted = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> watered = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dry = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dead = new NetworkVariable<bool>(false);

    void Awake() {
        plantPot = GetComponent<MeshFilter>().mesh;
        currentMesh = GetComponent<MeshFilter>();
        audioSource = GetComponent<AudioSource>();    
    }
    public override void Update() {
        base.Update();
        ChangeMeshServerRpc();
    }
    protected override void Interaction(){
        SetServerRpc(!Value);
        if (PlayerAvatar.IsHolding<Seed>()) {
            PlantingServerRpc();
            seed = PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY);
            PlayerManager.Instance.LocalPlayer.Avatar.DropItem(PlayerAvatar.Slot.PRIMARY);
           /*  audioSource.PlayOneShot(plantSound); */
        }
        if (PlayerAvatar.IsHolding<WateringCan>()){
            if(seedPlanted.Value){
                WaterPlantServerRpc(true);
                Debug.Log("plant has been watered");   
            } 
        }
        if (seed != null) {
            DespawnServerRpc();
        }
        
    }
      public void OnMeshChange(int previous, int current){
        if (m_mesh.Value == 1){
            currentMesh.mesh = plantPot;
        }
        if (m_mesh.Value == 2){
            Debug.Log("seed in pot");
            currentMesh.mesh = seedInPot;
        }
        if (m_mesh.Value == 3){
            currentMesh.mesh = plantStage1;
        }
        if (m_mesh.Value == 4){
            currentMesh.mesh = dryPlant;
        }
        if (m_mesh.Value == 5){
            currentMesh.mesh = deadPlant;
        }
     } 
    public override void OnStateChange(bool previous, bool current) {
        if(seedPlanted.Value && (!notPlanted.Value) ){
            if (!grown.Value && watered.Value){
                GrowPlantServerRpc();
            }
        } 
        if (dry.Value && (!watered.Value)){
            PlantDyingServerRpc();
        }
        if (dead.Value) {
            PlantDeadServerRpc();
        }
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

    IEnumerator WaitForPlantGrow(float maxtime){
        float growIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant grows in " + growIn + "sec");
        yield return new WaitForSeconds(growIn);
        grown.Value = true;
    }
    IEnumerator TimeTillPlantDry(float maxtime){
        float dryIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant ist dry in " + dryIn + "secs");
        yield return new WaitForSeconds(dryIn);
        watered.Value = false;
        dry.Value = true;
    }
    IEnumerator TimeTillPlantDead(float maxtime){
        float deadIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant is dead in " + deadIn + " secs");
        yield return new WaitForSeconds(deadIn);
        dead.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc() {
        seed.GetComponentInParent<NetworkObject>().Despawn(destroy: true);   
    }
    [ServerRpc(RequireOwnership = false)]
    public void GrowPlantServerRpc() {
        Debug.Log("growing seed");
        if (!dry.Value && watered.Value) {
            StartCoroutine(WaitForPlantGrow(60));
        } else StopCoroutine(WaitForPlantGrow(60));
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlantingServerRpc() {
        seedPlanted.Value = true;
        notPlanted.Value = false;
    }
    [ServerRpc(RequireOwnership = false)]
    public void WaterPlantServerRpc(bool wet) {
        watered.Value = wet;
        dry.Value = !wet;
        Debug.Log("plant watered");
        StartCoroutine(TimeTillPlantDry(60));
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlantDyingServerRpc() {
        if (dry.Value && !watered.Value){
            StartCoroutine(TimeTillPlantDead(60));
        } else StopCoroutine(TimeTillPlantDead(60));
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlantDeadServerRpc() {
        seedPlanted.Value = false;
        watered.Value = false;
        dry.Value = false;
        dead.Value = false;
        grown.Value = false;
        notPlanted.Value = true;
        StopCoroutine(TimeTillPlantDead(60));
        StopCoroutine(TimeTillPlantDry(60));
        StopCoroutine(WaitForPlantGrow(60));
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeMeshServerRpc() {
        if (dead.Value) {
            m_mesh.Value = 6;
        }
        if (seedPlanted.Value && !notPlanted.Value) {
            Debug.Log("seed is in pot");
            m_mesh.Value = 2;
        }
        if (grown.Value && watered.Value && !dry.Value) {
            m_mesh.Value = 3;
        }
        if (dry.Value && (!watered.Value)) {
            m_mesh.Value = 4;
        }
        if (notPlanted.Value){
            m_mesh.Value = 1;
        }
        
        
    }
    
    
    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        dry.OnValueChanged += OnStateChange;
        dead.OnValueChanged += OnStateChange;
        watered.OnValueChanged += OnStateChange;
        seedPlanted.OnValueChanged += OnStateChange;
        m_mesh.OnValueChanged += OnMeshChange;
        grown.OnValueChanged += OnStateChange;
        notPlanted.OnValueChanged += OnStateChange;
        OnMeshChange(m_mesh.Value, m_mesh.Value);
        OnStateChange(false, false);
    } 
    public override void OnNetworkDespawn(){
        dry.OnValueChanged -= OnStateChange;
        dead.OnValueChanged -= OnStateChange;
        watered.OnValueChanged -= OnStateChange;
        seedPlanted.OnValueChanged -= OnStateChange;
        m_mesh.OnValueChanged -= OnMeshChange;
        grown.OnValueChanged -= OnStateChange;
        notPlanted.OnValueChanged -= OnStateChange;
    }
}
