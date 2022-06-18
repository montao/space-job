using UnityEngine;

public class PlayerRagdoll : MonoBehaviour {
    private Collider[] m_RagdollColliders;
    private Rigidbody[] m_RagdollRigidbodies;
    private PlayerAvatar m_Avatar;
    bool m_IsSetup = false;

    public void Setup(PlayerAvatar avatar, GameObject character) {
        m_RagdollColliders = character.GetComponentsInChildren<Collider>();
        m_RagdollRigidbodies = character.GetComponentsInChildren<Rigidbody>();
        m_Avatar = avatar;

        Debug.Log("Ragdoll:  Found " + m_RagdollColliders.Length + " colliders and "
                + m_RagdollRigidbodies.Length + " rigidbodies.");
        m_IsSetup = true;
    }

    public void SetRagdollEnabled(bool ragdoll_enabled) {
        if (!m_IsSetup) {
            Debug.LogError("Ragdoll " + name + " has not been Setup yet!");
        }

        foreach (var c in m_RagdollColliders) {
            c.enabled = ragdoll_enabled;
        }
        foreach (var r in m_RagdollRigidbodies) {
            r.isKinematic = !ragdoll_enabled;
        }
        m_Avatar.AnimationController.enabled = !ragdoll_enabled;
    }
}
