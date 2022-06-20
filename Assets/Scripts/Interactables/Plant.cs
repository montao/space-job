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

    void Awake() {
        currentMesh = GetComponent<MeshFilter>();
        audioSource = GetComponent<AudioSource>();    
    }

    protected override void Interaction(){
        SetServerRpc(!Value);
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
                WaterPlantServerRpc();
            }
            
        }
        DespawnServerRpc();
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
        Debug.Log(growIn);
        yield return new WaitForSeconds(growIn);
        Debug.Log("change mesh");
        currentMesh.mesh = plantStage1;
    }
    IEnumerator TimeTillPlantDry(float maxtime){
        float dryIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log(dryIn);
        yield return new WaitForSeconds(dryIn);
        Debug.Log("change mesh");
        healthyPlant = currentMesh.mesh;
        currentMesh.mesh = dryPlant;
        watered.Value = false;
    }
    IEnumerator TimeTillPlantDead(float maxtime){
        float deadIn = UnityEngine.Random.Range(2, maxtime);
        Debug.Log(deadIn);
        yield return new WaitForSeconds(deadIn);
        Debug.Log("change mesh");
        currentMesh.mesh = deadPlant;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc() {
        seed.GetComponentInParent<NetworkObject>().Despawn(destroy: true);   
    }
    [ServerRpc(RequireOwnership = false)]
    public void GrowPlantServerRpc() {
        if(seedPlanted.Value) {
            Debug.Log("starting Plant Corountine");
            StartCoroutine(WaitForPlantGrow(10));
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void WaterPlantServerRpc() {
        if(currentMesh.mesh != dryPlant){
            currentMesh.mesh = healthyPlant;
        }
        Debug.Log("starting Plant Corountine");
        StartCoroutine(TimeTillPlantDry(10));
        if(!watered.Value){
            Debug.Log("Plant starting to die");
            StartCoroutine(TimeTillPlantDead(10));
        }
    }

}
