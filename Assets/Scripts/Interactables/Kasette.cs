using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kasette : DroppableInteractable{
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        AudioClip sound = GetRandomInteractionClip();
        audioSource.PlayOneShot(sound);
        return PlayerAnimation.INTERACT;
    }

}
