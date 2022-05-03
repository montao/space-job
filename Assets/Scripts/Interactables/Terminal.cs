using TMPro;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class Terminal : Interactable<FixedString32Bytes> {
    public TMP_Text Text;
    private string m_ErrorText;
    private string m_TextEntered = "";

    public static readonly Dictionary<KeyCode, string> KEYCODES = new Dictionary<KeyCode, string>{
        {KeyCode.Alpha0, "0"},
        {KeyCode.Alpha1, "1"},
        {KeyCode.Alpha2, "2"},
        {KeyCode.Alpha3, "3"},
        {KeyCode.Alpha4, "4"},
        {KeyCode.Alpha5, "5"},
        {KeyCode.Alpha6, "6"},
        {KeyCode.Alpha7, "7"},
        {KeyCode.Alpha8, "8"},
        {KeyCode.Alpha9, "9"},
        {KeyCode.A, "A"},
        {KeyCode.B, "B"},
        {KeyCode.C, "C"},
        {KeyCode.D, "D"},
        {KeyCode.E, "E"},
        {KeyCode.F, "F"},
    };

    public override void OnStateChange(FixedString32Bytes previous, FixedString32Bytes current) {
        UpdateText();
    }

    protected override void Interaction() {
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetEnteredTextServerRpc(string text, bool entered) {
        if (entered) {
            ShipManager.Instance.TryResolvePowerOutageEvent(Value.ToString());
            m_State.Value = "";
        } else {
            m_State.Value = text;
        }
    }

    public void DisplayError(string err) {
        m_ErrorText = err;
        UpdateText();
    }
    private void UpdateText() {
        Text.text = m_ErrorText;
        if (!ShipManager.Instance.HasPower) {
        Text.text += "\n>" + Value + "|";
        }
    }

    public override void Update() {
        base.Update();
        if (m_IsInArea && !ShipManager.Instance.HasPower) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SetEnteredTextServerRpc(m_TextEntered, true);
                m_TextEntered = "";
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                if (m_TextEntered.Length > 0) {
                    m_TextEntered = m_TextEntered.Remove(m_TextEntered.Length - 1, 1);
                    SetEnteredTextServerRpc(m_TextEntered, false);
                }
            }
            foreach(var kv in KEYCODES) {
                var keycode = kv.Key;
                if (Input.GetKeyDown(keycode)) {
                    m_TextEntered += kv.Value;
                    SetEnteredTextServerRpc(m_TextEntered, false);
                }
            }
        }
    }
}
