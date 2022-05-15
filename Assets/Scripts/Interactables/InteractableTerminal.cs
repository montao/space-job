using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTerminal : InteractableBase
{
    public TerminalCanvas display;
    private Canvas m_TargetCanvas;
    private void Awake() {
        m_TargetCanvas = GetComponentInChildren<Canvas>();
        display.transform.SetParent(m_TargetCanvas.transform, false);
    }
    // Start is called before the first frame update
    protected override void Interaction(){

    }
}
