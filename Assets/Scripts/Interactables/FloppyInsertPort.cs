using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class FloppyInsertPort : InteractableBase
{
    private NetworkVariable<NetworkObjectReference> m_PlayerData = new NetworkVariable<NetworkObjectReference>();
    protected override void Interaction(){
        if (PlayerAvatar.IsHolding<RevivalFloppy>()) {
            DroppableInteractable item = Util.GetDroppableInteractable(PlayerManager.Instance.LocalPlayer.Avatar.GetInventoryItem(PlayerAvatar.Slot.PRIMARY));
            RevivalFloppy floppy = item.GetComponent<RevivalFloppy>();
            m_PlayerData.Value = floppy.Player.Value;
            floppy.DespawnServerRpc();
            Debug.Log("Cromch");
        }
    }
}
