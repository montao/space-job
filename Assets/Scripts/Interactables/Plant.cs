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
    public NetworkVariable<bool> seedPlanted = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> watered = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dry = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> dead = new NetworkVariable<bool>(false);

    void Awake() {
        plantPot = GetComponent<MeshFilter>().mesh;
        currentMesh = GetComponent<MeshFilter>();
        audioSource = GetComponent<AudioSource>();    
    }
    private void Update() {
        // Debug.Log(watered.Value);
/*         if(dry.Value && (!watered.Value)) {
            PlantDyingServerRpc();
        }

        if(seedPlanted.Value && watered.Value){
            GrowPlantServerRpc();
        } */
        
    }

    protected override void Interaction(){
        SetServerRpc(!Value);
        if(dead.Value){
            ChangeMeshServerRpc(1);
            seedPlanted.Value = false;
            watered.Value = false;
            dry.Value = false;
            dead.Value = false;
        }
        if (PlayerAvatar.IsHolding<Seed>()) {
            seed = PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY);
            ChangeMeshServerRpc(2);
            Debug.Log("change plant mesh");
            PlayerManager.Instance.LocalPlayer.Avatar.DropItem(PlayerAvatar.Slot.PRIMARY);
            seedPlanted.Value = true;
           /*  audioSource.PlayOneShot(plantSound); */
        }
        if (PlayerAvatar.IsHolding<WateringCan>()){
            if(seedPlanted.Value){
                watered.Value = true;
                Debug.Log("plant has been watered");
                WaterPlantServerRpc(Value);
            }
            
        }
        if (seed != null) {
            DespawnServerRpc();
        }
        GrowPlantServerRpc(Value);
    }


    public override void OnStateChange(bool previous, bool current) {
        PlantDyingServerRpc(current);
        GrowPlantServerRpc(current);
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
        ChangeMeshServerRpc(3);
        dry.Value = true;
    }
    IEnumerator TimeTillPlantDry(float maxtime){
        float dryIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant ist dry in " + dryIn + "secs");
        yield return new WaitForSeconds(dryIn);
        Debug.Log("changing into dry plant mesh");
        ChangeMeshServerRpc(4);
        watered.Value = false;
        dry.Value = true;
    }
    IEnumerator TimeTillPlantDead(float maxtime){
        float deadIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log("plant is dead in " + deadIn + " secs");
        yield return new WaitForSeconds(deadIn);
        Debug.Log("dead plant mesh");
        ChangeMeshServerRpc(6);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc() {
        seed.GetComponentInParent<NetworkObject>().Despawn(destroy: true);   
    }
    [ServerRpc(RequireOwnership = false)]
    public void GrowPlantServerRpc(bool smol) {
        if(seedPlanted.Value && smol && watered.Value) {
            Debug.Log("growing seed");
            StartCoroutine(WaitForPlantGrow(10));
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void WaterPlantServerRpc(bool wet) {
        if(wet){
            Debug.Log("plant watered");
            StartCoroutine(TimeTillPlantDry(10));
        } 
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlantDyingServerRpc(bool dying) {
        if(dying && dry.Value && (!watered.Value)){
            Debug.Log("Plant starting to die");
            StartCoroutine(TimeTillPlantDead(10));
        }
        
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeMeshServerRpc(int mesh) {
        Debug.Log("changed mesh");
        if (mesh == 1){
            currentMesh.mesh = plantPot;
        }
        if (mesh == 2){
            currentMesh.mesh = seedInPot;
        }
        if (mesh == 3){
            currentMesh.mesh = plantStage1;
        }
        if (mesh == 4){
            currentMesh.mesh = dryPlant;
        }
        if (mesh == 5){
            currentMesh.mesh = healthyPlant;
        }
        if (mesh == 6){
            currentMesh.mesh = deadPlant;
        }
        
    }
    

}
