using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumPadButton : InteractableBase
{
    public int Number;
    public NumPad VendingNumPad;
    protected override void Interaction(){
        if(Number == 420){
            VendingNumPad.ClearInput();
        }
        else if(Number == 69){
            VendingNumPad.ClearInput();
        }
        else{
            VendingNumPad.DisplayInputText(Number);
        }
    }
    public override float CooldownTime()
    {
        return 0.0f;
    }
}
