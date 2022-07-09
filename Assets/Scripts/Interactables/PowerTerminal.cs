using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if(m_InCounter == 6){
            if (ShipManager.Instance.TryResolvePowerOutageEvent(input_number.ToString()))
            ClearInput();
        }
        InputDisplay.text += input_number;
        m_InCounter ++;
    }
    public void ClearInput(){
        InputDisplay.text = ">";
        m_InCounter = 0;
    }
    public void DisplayError(string err) {
        Debug.Log("Error Displayed");
        ErrorText.text = err;
    }
}