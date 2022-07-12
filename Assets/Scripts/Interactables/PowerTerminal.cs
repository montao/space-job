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
        if(m_InCounter == 4){
            CheckIfPowerIsResolvedServerRPC();
            ClearInput();
        }
        InputDisplay.text += input_number;
        m_InCounter ++;
    }
    public void ClearInput(){
        InputDisplay.text = ">";
        m_InCounter = 0;
    }
    [ServerRpc(RequireOwnership = false)]
    public void CheckIfPowerIsResolvedServerRPC(){
        ShipManager.Instance.TryResolvePowerOutageEvent(InputDisplay.text);
    }
    public void DisplayError(string err) {
        Debug.Log("Error Displayed");
        ErrorText.text = err;
    }
}