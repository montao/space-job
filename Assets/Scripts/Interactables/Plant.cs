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
    private NetworkVariable<bool> seedPlanted = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> watered = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dry = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dead = new NetworkVariable<bool>(false);

    void Awake() {
        plantPot = GetComponent<MeshFilter>().mesh;
        currentMesh = GetComponent<MeshFilter>();
        audioSource = GetComponent<AudioSource>();    
    }
    private void Update() {
        Debug.Log(watered.Value);
        if(dry.Value && (!watered.Value)) {
            PlantDyingServerRpc();
        }

        if(seedPlanted.Value && watered.Value){
            GrowPlantServerRpc();
        }
        
    }

    protected override void Interaction(){
        SetServerRpc(!Value);
        if(dead.Value){
            currentMesh.mesh = plantPot;
            seedPlanted.Value = false;
            watered.Value = false;
            dry.Value = false;
            dead.Value = false;
        }
        if (PlayerAvatar.IsHolding<Seed>()) {
            seed = PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY);
            currentMesh.mesh = seedInPot;
            Debug.Log("change plant mesh");
            PlayerManager.Instance.LocalPlayer.Avatar.DropItem(PlayerAvatar.Slot.PRIMARY);
            seedPlanted.Value = true;
           /*  audioSource.PlayOneShot(plantSound); */
        }
        if (PlayerAvatar.IsHolding<WateringCan>()){
            if(seedPlanted.Value){
                watered.Value = true;
                Debug.Log("plant has been watered");
                WaterPlantServerRpc();
            }
            
        }
        if (seed != null) {
            DespawnServerRpc();
        }
        GrowPlantServerRpc();
    }

    public override void OnStateChange(bool previous, bool current) {
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;
    }

    IEnumerator WaitForPlantGrow(float maxtime){
        float growIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant grows in " + growIn + "sec");
        yield return new WaitForSeconds(growIn);
        Debug.Log("change into stage 1 plant mesh");
        currentMesh.mesh = plantStage1;
        dry.Value = true;
    }
    IEnumerator TimeTillPlantDry(float maxtime){
        float dryIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant ist dry in " + dryIn + "secs");
        yield return new WaitForSeconds(dryIn);
        Debug.Log("changing into dry plant mesh");
        currentMesh.mesh = dryPlant;
        watered.Value = false;
        dry.Value = true;
    }
    IEnumerator TimeTillPlantDead(float maxtime){
        float deadIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant is dead in " + deadIn + " secs");
        yield return new WaitForSeconds(deadIn);
        Debug.Log("dead plant mesh");
        currentMesh.mesh = deadPlant;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc() {
        seed.GetComponentInParent<NetworkObject>().Despawn(destroy: true);   
    }
    [ServerRpc(RequireOwnership = false)]
    public void GrowPlantServerRpc() {
        if(seedPlanted.Value) {
            Debug.Log("growing seed");
            StartCoroutine(WaitForPlantGrow(10));
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void WaterPlantServerRpc() {
        Debug.Log("plant watered");
        StartCoroutine(TimeTillPlantDry(10));
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlantDyingServerRpc() {
        Debug.Log("Plant starting to die");
        StartCoroutine(TimeTillPlantDead(10));
        
    }
    

}