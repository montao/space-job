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
        List<Transform> possibleSpawnLocations = new List<Transform>();
        foreach(Transform fire_location in room.m_FireSpawnLocations){
            Vector3 playerPos = PlayerManager.Instance.LocalPlayer.Avatar.transform.position;
            if((playerPos - fire_location.position).magnitude <= 3){
                possibleSpawnLocations.Add(fire_location);
            }
        }
        room.FireJumpOver(possibleSpawnLocations);
        return PlayerAnimation.INTERACT;
    }

    public override string FriendlyName() {
        return "Flamethrower";
    }
}
