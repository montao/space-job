using UnityEngine;
using Unity.Netcode;

public class DeathZone : MonoBehaviour
{
    private float m_DamagePerTick = 0.01f;
    private Collider DeathField;
    private void Start() {
        DeathField = GetComponent<Collider>();
    }

    public void SetDamage(float damage){
        m_DamagePerTick = damage;
    }

    private void OnTriggerStay(Collider other) {
        var ava = other.gameObject.GetComponent<PlayerAvatar>();
        if (ava != null && ava.OwnerClientId == NetworkManager.Singleton.LocalClientId) {
            if (ava.m_Health.Value > 0) {
                ava.TakeDamage(m_DamagePerTick);
            }
        }
    }
}
