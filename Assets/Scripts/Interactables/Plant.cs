using UnityEngine;
using Unity.Netcode;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

public class Plant : Interactable<bool> {
    [SerializeField]
    private AudioClip plantSound;
    private AudioSource audioSource;
    [SerializeField]
    protected GameObject[] plantList;
    [SerializeField]
    protected Transform seedPlantedTrans;
    [SerializeField]
    protected Transform seedNotPlantedTrans;
/*     [SerializeField]
    protected Transform seedSpawn; */
    private GameObject seed;
    private GameObject plant;
    private GameObject deadPlant;
    private GameObject dryPlant;
/*     private NetworkObject seed;
 */ public NetworkVariable<int> m_mesh = new NetworkVariable<int>(1);
    public NetworkVariable<bool> notPlanted = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> grown = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> seedPlanted = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> watered = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dry = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dead = new NetworkVariable<bool>(false);

    void Awake() {
        /* plantList = GameObject.FindGameObjectsWithTag("PlantList"); */
        seed = plantList[0];
        plant = plantList[1];
        dryPlant = plantList[2];
        deadPlant = plantList[3];
        audioSource = GetComponent<AudioSource>();    
    }
    public override void Update() {
        base.Update();
        /* ChangePlantServerRpc(); */
    }
    protected override void Interaction(){
        SetServerRpc(!Value);
        if (PlayerAvatar.IsHolding<Seed>()) {
            PlantingServerRpc();
            /* seed = PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY); */
            seed.transform.position = seedPlantedTrans.transform.position;
            seed.transform.rotation = seedPlantedTrans.transform.rotation;
            seed.transform.localScale = seedPlantedTrans.transform.localScale;
           /*  audioSource.PlayOneShot(plantSound); */
        }
        if (PlayerAvatar.IsHolding<WateringCan>()){
            if(seedPlanted.Value){
                WaterPlantServerRpc(true);
                Debug.Log("plant has been watered");   
            } 
        }
/*         if (seed != null) {
            DespawnServerRpc();
        } */
        
    }
      public void OnPlantChange(bool previous, bool current){
        if (dead.Value) {
            deadPlant.SetActive(current);
        }
        if (seedPlanted.Value && !notPlanted.Value) {
            seed.SetActive(current);
            Debug.Log("seed is in pot");

            /* m_mesh.Value = 2; */
        }
        if (grown.Value && watered.Value && !dry.Value) {
            plant.SetActive(current);
            /* m_mesh.Value = 3; */
        }
        if (dry.Value && (!watered.Value)) {
            dryPlant.SetActive(current);
            /* m_mesh.Value = 4; */
        }
        if (notPlanted.Value){
            seed.transform.position = seedNotPlantedTrans.transform.position;
            seed.transform.rotation = seedNotPlantedTrans.transform.rotation;
            seed.transform.localScale = seedNotPlantedTrans.transform.localScale;
            /* m_mesh.Value = 1; */
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

/*     [ServerRpc(RequireOwnership = false)]
        public void DespawnServerRpc() {
        seed.GetComponentInParent<NetworkObject>().Despawn(destroy: true);   
    } */
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
    public void ChangePlantServerRpc() {
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
        dry.OnValueChanged += OnPlantChange;
        dead.OnValueChanged += OnStateChange;
        dead.OnValueChanged += OnPlantChange;
        watered.OnValueChanged += OnStateChange;
        watered.OnValueChanged += OnPlantChange;
        seedPlanted.OnValueChanged += OnStateChange;
        seedPlanted.OnValueChanged += OnPlantChange;
        grown.OnValueChanged += OnStateChange;
        grown.OnValueChanged += OnPlantChange;
        /* m_mesh.OnValueChanged += OnPlantChange; */
        notPlanted.OnValueChanged += OnStateChange;
        OnPlantChange(false, false);
        OnStateChange(false, false);
    } 
    public override void OnNetworkDespawn(){
        dry.OnValueChanged -= OnStateChange;
        dead.OnValueChanged -= OnStateChange;
        watered.OnValueChanged -= OnStateChange;
        seedPlanted.OnValueChanged -= OnStateChange;
        grown.OnValueChanged -= OnStateChange;
       /*  m_mesh.OnValueChanged -= OnPlantChange; */
        notPlanted.OnValueChanged -= OnStateChange;
    }
}
