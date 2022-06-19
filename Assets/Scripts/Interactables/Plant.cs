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
    private MeshFilter currentMesh;
    private NetworkObject seed;
    private NetworkVariable<bool> seedPlanted = new NetworkVariable<bool>(false);

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
        DespawnServerRpc();
        GrowPlantServerRpc();
    }

    public override void OnStateChange(bool previous, bool current) {
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;

        /* if(!prevCup || prevCup.GetComponentInChildren<CoffeCup>().IsPickedUp()) {
            Vector3 yeet = Vector3.Normalize(transform.forward + transform.up) * 50f;

            GameObject freshCup = Instantiate(cupPrefab, machine.GetComponent<Transform>().position, Quaternion.identity);
            freshCup.GetComponent<Rigidbody>().AddForce(yeet);
            freshCup.GetComponent<NetworkObject>().Spawn();
            prevCup = freshCup;
            Debug.Log("new cup");
        } */
    }

     IEnumerator WaitForPlantGrow(float maxtime){
        float growIn = UnityEngine.Random.Range(60, maxtime);
        Debug.Log(growIn);
        yield return new WaitForSeconds(growIn);
        Debug.Log("change mesh");
        currentMesh.mesh = plantStage1;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc() {
        seed.GetComponentInParent<NetworkObject>().Despawn(destroy: true);   
    }
    [ServerRpc(RequireOwnership = false)]
    public void GrowPlantServerRpc() {
        if(seedPlanted.Value) {
            Debug.Log("starting Plant Corountine");
            StartCoroutine(WaitForPlantGrow(120));
        }
    }

}
