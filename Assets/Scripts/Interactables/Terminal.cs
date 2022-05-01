using TMPro;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;

public class Terminal : Interactable<FixedString32Bytes> {
    public TMP_Text Text;
    private string m_ErrorText;
    private string m_TextEntered = "";

    public override void OnStateChange(FixedString32Bytes previous, FixedString32Bytes current) {
        UpdateText();
    }

    protected override void Interaction() {
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetEnteredTextServerRpc(string text, bool entered) {
        if (entered) {
            if (text == "uwu") {
                ShipManager.Instance.ResolvePowerOutageEvent();
            }
            m_State.Value = "";
        } else {
        m_State.Value = "> " + text;
        }
    }

    public void DisplayError(string err) {
        m_ErrorText = err;
        UpdateText();
    }
    private void UpdateText() {
        Text.text = m_ErrorText + "\n" + Value + (ShipManager.Instance.HasPower ? "" : "|");
    }

    public override void Update() {
        base.Update();
        if (m_IsInArea && !ShipManager.Instance.HasPower) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SetEnteredTextServerRpc(m_TextEntered, true);
                m_TextEntered = "";
            }
            if (Input.GetKeyDown(KeyCode.U)) {
                m_TextEntered += "u";
                SetEnteredTextServerRpc(m_TextEntered, false);
            }
            if (Input.GetKeyDown(KeyCode.W)) {
                m_TextEntered += "w";
                SetEnteredTextServerRpc(m_TextEntered, false);
            }
        }
    }
}
