using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class Flamethrower : DroppableInteractable {
    private Transform m_NearestFire;
    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        interactionAudioSource.gameObject.transform.position = PlayerManager.Instance.LocalPlayer.Avatar.gameObject.transform.position;
        AudioClip sound = GetRandomInteractionClip();
        interactionAudioSource.PlayOneShot(sound);
        Room room = PlayerManager.Instance.LocalPlayer.Avatar.CurrentRoom;
        foreach(Transform fire_location in room.m_FireSpawnLocations){
            float closest_Fire;
        }
        return PlayerAnimation.INTERACT;
    }

    public override string FriendlyName() {
        return "Flamethrower";
    }
}
