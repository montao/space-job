using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class HullBreachInstance : RangedInteractableBase {

    private Room m_Room;
    private Transform m_SpawnLocation;
    private EventParameters.HullBreachSize m_Size = EventParameters.HullBreachSize.SMALL;
    private bool m_PlayerIsHoldingPlate = false;  // server-only!
    private Coroutine m_GrowCoroutine;

    private readonly float LARGE_HULLBREACH_SCALE = 3.2f;


    public float DrainFactor() {
        return m_Size == EventParameters.HullBreachSize.LARGE ? 1.8f : 0.7f;
    }

    public override void OnDestroy() {
        PlayerManager.Instance.LocalPlayer.Avatar.ReleaseMovementLock(GetHashCode());
        base.OnDestroy();
    }

    public override float CooldownTime() {
        return 2.0f;
    }

    protected override void Interaction() {
        if (m_Size == EventParameters.HullBreachSize.SMALL) {
            InteractionSmall();
        } else {
            InteractionLarge();
        }
    }

    private void InteractionSmall() {
        if (PlayerAvatar.IsHolding<BreachBeGone>()) {
            ResolvedServerRpc();
        }
    }

    private void InteractionLarge() {
        if (PlayerAvatar.IsHolding<MetalPlate>()) {
            StartCoroutine(HoldPlate());
        } else if (PlayerAvatar.IsHolding<PipeWrench>()) {  // TODO add welder
            AttemptWeldPlateServerRpc();
        }
    }

    private IEnumerator HoldPlate() {
        // TODO: Trigger a holding animation
        PlayerManager.Instance.LocalPlayer.Avatar.LockMovement(GetHashCode());
        SetIsHoldingServerRpc(true);
        yield return new WaitForSeconds(2.0f);
        PlayerManager.Instance.LocalPlayer.Avatar.ReleaseMovementLock(GetHashCode());
        SetIsHoldingServerRpc(false);
    }

    // called by room
    public void Setup(Room room, Transform spawn) {
        m_Room = room;
        m_SpawnLocation = spawn;
        if (IsServer) {
            m_GrowCoroutine = StartCoroutine(Grow());
        }
    }

    private IEnumerator Grow() {
        yield return new WaitForSeconds(Random.Range(10f, 20f));
        GrowHoleClientRpc(EventParameters.HullBreachSize.LARGE);
    }

    [ClientRpc]
    public void GrowHoleClientRpc(EventParameters.HullBreachSize new_size) {
        m_Size = new_size;
        if (new_size == EventParameters.HullBreachSize.LARGE) {
            transform.localScale = LARGE_HULLBREACH_SCALE * transform.localScale;
        }
    }

    [ServerRpc(RequireOwnership=false)]
    public void ResolvedServerRpc() {
        Debug.Log("Hull Breach fixed in " + m_Room);
        StopAllCoroutines();
        m_Room.HullBreachResolved(this, m_SpawnLocation);
        GetComponent<NetworkObject>().Despawn(destroy: true);  // breach do be gone
    }

    [ServerRpc(RequireOwnership=false)]
    public void SetIsHoldingServerRpc(bool holding) {
        m_PlayerIsHoldingPlate = holding;
    }

    [ServerRpc(RequireOwnership=false)]
    public void AttemptWeldPlateServerRpc() {
        if (m_PlayerIsHoldingPlate) {
            ResolvedServerRpc();
        }
    }
}
