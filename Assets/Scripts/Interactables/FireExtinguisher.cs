using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class FireExtinguisher : DroppableInteractable {
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        AudioClip sound = GetRandomInteractionClip();
        audioSource.PlayOneShot(sound);
        return PlayerAnimation.INTERACT;
    }
}
