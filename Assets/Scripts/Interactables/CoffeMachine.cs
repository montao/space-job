using UnityEngine;
using Unity.Netcode;

public class CoffeMachine : Interactable<bool> {
    public GameObject cupPrefab;
    public GameObject machine;
    private GameObject prevCup = null;
   
    protected override void Interaction(){
        SetServerRpc(!Value);
        if (PlayerAvatar.IsHolding<PipeWrench>()) {
            transform.localScale = transform.localScale * 1.6f;
        }
    }

    public override void OnStateChange(bool previous, bool current) {
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetServerRpc(bool value){
        m_State.Value = value;

        if(!prevCup || prevCup.GetComponentInChildren<CoffeCup>().IsPickedUp()) {
            Vector3 yeet = Vector3.Normalize(transform.forward + transform.up) * 50f;

            GameObject freshCup = Instantiate(cupPrefab, machine.GetComponent<Transform>().position, Quaternion.identity);
            freshCup.GetComponent<Rigidbody>().AddForce(yeet);
            freshCup.GetComponent<NetworkObject>().Spawn();
            prevCup = freshCup;
            Debug.Log("new cup");
        }
    }
    private void Awake() {
    }
}
