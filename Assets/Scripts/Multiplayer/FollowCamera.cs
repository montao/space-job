using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    PlayerAvatar player;

    // Update is called once per frame
    void Update() {
        string name = "";
        foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
            if (player.IsOwner) {
                name = player.PlayerName;
            }
        }

        foreach (var player in FindObjectsOfType<PlayerAvatar>()) {
            if (player.nameText.text == name) {
                transform.LookAt(player.transform.position);
                return;
            }
        }

    }
}
