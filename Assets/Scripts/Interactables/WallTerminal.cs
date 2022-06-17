using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTerminal : TwoLevelInteractable
{
    private Animator Visor;
    public override void Start() {
        base.Start();
        Visor = GetComponent<Animator>();
        if(m_InteractionRange)
        m_InteractionRange.OnRangeTriggerEnter += OnVisorTriggerEnter;
        m_InteractionRange.OnRangeTriggerExit += OnVisorTriggerExit;
    }

    public void OnVisorTriggerEnter(Collider other){
        Visor.SetBool("active", true);
    }
    public void OnVisorTriggerExit(Collider other){
        Visor.SetBool("active", false);
    }
}
