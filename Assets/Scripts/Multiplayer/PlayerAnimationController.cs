using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public static class Animation{
    public const string INTERACT = "interact";
    public const string ARMWAVE = "armwave";
    public const string SIT = "sit";
    public const string JUMP = "jump";
    public const string DRINK = "drink";
}

public class PlayerAnimationController : NetworkBehaviour {
    private Animator animation;
    [ClientRpc]
    public void TriggerAnimationClientRpc(string animation){
        
    }
}