using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScreen : NetworkBehaviour {

    public TMP_Text status;

    [SerializeField]
    private TMP_Text m_ButtonText;

    private Button[] m_Buttons;
    private Canvas m_Canvas;
    private NetworkVariable<int> m_ReadyCount = new NetworkVariable<int>(0);
    private List<ulong> m_Ready = new List<ulong>();

    private string won = "Conratulations:\nYou Did It,\nEmployees!";
    private string lost = "Mission Failed:\nUnfortunately All Employees Are MIA";

    void Start() {
        m_Canvas = GetComponent<Canvas>();
        m_Buttons = GetComponentsInChildren<Button>(includeInactive: true);
    }

    void Update() {
        int n_ready = m_ReadyCount.Value;
        int n_total = PlayerManager.Instance.ConnectedPlayerCount;
        string text = "Return to Lobby (" + n_ready + "/" + n_total +")";
        m_ButtonText.text = text;

        if (IsServer && n_ready >= n_total) {
            ShipManager.Instance.SetWon();
            ShipManager.Instance.StartNewGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetReadyServerRpc(ulong client_id) {
        if (m_Ready.Contains(client_id)) {
            return;
        }

        m_Ready.Add(client_id);
        m_ReadyCount.Value = m_ReadyCount.Value + 1;
    }

    public void OnButtonPress() {
        SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    public void SetEnabled(bool enabled, bool success) {
        status.text = (success) ? won : lost;
        if (m_Buttons != null) {
            foreach (var button in m_Buttons) {
                if (button) {
                    button.gameObject.SetActive(enabled);
                }
            }
        }
        if (m_Canvas != null) {
            m_Canvas.enabled = enabled;
        }
    }
}
