using UnityEngine;
using Unity.Netcode;

public class FireInstance : RangedInteractableBase
{
    private float m_DamagePerTick;
    private Collider DeathField;
    public override void Start() {
        base.Start();
        DeathField = GetComponent<Collider>();
    }

    protected override void Interaction() {
        
    }

    private void OnTriggerStay(Collider other) {
        var ava = other.gameObject.GetComponent<PlayerAvatar>();
        if (ava != null && ava.OwnerClientId == NetworkManager.Singleton.LocalClientId) {
            if (ava.m_Health.Value > 0) {
                ava.TakeDamage(0.01f);
            }
        }
    }
}
