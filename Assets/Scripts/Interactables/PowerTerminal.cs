using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PowerTerminal : TwoLevelInteractable
{
    public TMP_Text InputDisplay;
    public TMP_Text ErrorText;
    private string m_ErrorText;
    private int m_InCounter = 0;
    public override void Start() {
        base.Start();
        ClearInput();   
    }

    public override void Update()
    {
        base.Update();

    }
    public void DisplayInputText(int input_number){
        InputDisplay.text += input_number;
        m_InCounter++;
        if (m_InCounter >= 6){
            CheckIfPowerIsResolvedServerRpc();
            ClearInput();
        }
    }
    public void ClearInput(){
        InputDisplay.text = ">";
        m_InCounter = 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckIfPowerIsResolvedServerRpc(){
        int number = Int32.Parse(InputDisplay.text.Substring(1));
        Debug.Log("Trying to resolve power outage with: " + number);
        ShipManager.Instance.TryResolvePowerOutageEvent(number);
    }

    public void DisplayError(int room) {
        ErrorText.text = "" + room;
    }
}
