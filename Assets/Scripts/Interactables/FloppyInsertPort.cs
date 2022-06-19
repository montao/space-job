using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class FloppyInsertPort : InteractableBase
{
    private NetworkVariable<NetworkObjectReference> m_PlayerData = new NetworkVariable<NetworkObjectReference>();
    protected override void Interaction(){
        PlayerAvatar activePlayer = PlayerManager.Instance.LocalPlayer.Avatar;
        DroppableInteractable thing = Util.GetDroppableInteractable(activePlayer.GetInventoryItem(PlayerAvatar.Slot.PRIMARY));
        Debug.Log(thing.name);
        thing.DropServerRpc(activePlayer.dropPoint.position);
        thing.DespawnServerRpc();
        if (PlayerAvatar.IsHolding<CoffeCup>()) {
            DroppableInteractable item = Util.GetDroppableInteractable(activePlayer.GetInventoryItem(PlayerAvatar.Slot.PRIMARY));
            RevivalFloppy floppy = item.GetComponent<RevivalFloppy>();
            m_PlayerData.Value = floppy.Player.Value;
            floppy.DespawnServerRpc();
            Debug.Log("Cromch");
        }
    }
}
