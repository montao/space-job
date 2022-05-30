using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/**
 * An interactable with child interactables.
 */
[RequireComponent(typeof(CameraSwap))]
public class TwoLevelInteractable : Interactable<int> {

    public static readonly int NOT_OCCUPIED = -1;

    // Will be active iff local player is interacting with the NavTerminal
    [SerializeField]
    private List<SecondaryButton> m_Buttons;

    private CameraSwap m_CameraSwap;

    private bool m_NeedsPowerInitial;

    public override void Start() {
        base.Start();
        m_CameraSwap = GetComponent<CameraSwap>();
        OnStateChange(NOT_OCCUPIED, NOT_OCCUPIED);
        if (IsServer) {
            m_State.Value = NOT_OCCUPIED;
        }
        m_NeedsPowerInitial = NeedsPower;
        SetSecondaryButtonsActive(false);
    }

    protected override bool PlayerCanInteract() {
        return base.PlayerCanInteract() && !LocalPlayerIsInteracting();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryToggleOccupiedServerRpc(int playerId) {
        if (Value == NOT_OCCUPIED) {  // no one at terminal
            m_State.Value = playerId;
        } else if (Value == playerId) {  // player left terminal
            m_State.Value = NOT_OCCUPIED;
        }
        // Otherwise, someone else is at the terminal.  it will remain occupied
    }

    protected override void Interaction() {
        TryToggleOccupiedServerRpc((int)NetworkManager.Singleton.LocalClientId);
    }

    public override void OnStateChange(int prev, int current) {
        // not ready yet
        if (!PlayerManager.Instance.LocalPlayer || !PlayerManager.Instance.LocalPlayer.Avatar) {
            return;
        }

        if (current == NOT_OCCUPIED) {
            if (prev == (int)NetworkManager.Singleton.LocalClientId) { // local player left terminal
                PlayerManager.Instance.LocalPlayer.Avatar.ReleaseMovementLock(GetHashCode());
                m_CameraSwap.SwitchAway();
                NeedsPower = m_NeedsPowerInitial;
                SetSecondaryButtonsActive(false);
            }
        } else {
            if (current == (int)NetworkManager.Singleton.LocalClientId) { // local player entered terminal
                m_CameraSwap.SwitchTo();
                PlayerManager.Instance.LocalPlayer.Avatar.LockMovement(GetHashCode());
                NeedsPower = false;  // allow exit
                SetSecondaryButtonsActive(true);
            }
        }
    }

    private void SetSecondaryButtonsActive(bool active) {
        foreach (var button in m_Buttons) {
            button.CanInteract = active;
        }
    }

    public override void Update() {
        base.Update();
        if (LocalPlayerIsInteracting()) { // local player at terminal
            if (Util.PlayerIsPressingMoveButton() || Input.GetKeyDown(KeyCode.Escape)) {
                TryExitLocalPlayer();
            }
        }
    }

    public void TryExitLocalPlayer() {
        if (LocalPlayerIsInteracting()) {
            TryToggleOccupiedServerRpc((int)NetworkManager.Singleton.LocalClientId);
        }
    }

    public bool LocalPlayerIsInteracting() {
        return m_State.Value == (int)NetworkManager.Singleton.LocalClientId;
    }
}
