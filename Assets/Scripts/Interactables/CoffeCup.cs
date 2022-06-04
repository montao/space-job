using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/**
  * Mjam, that's some good coffee~

  Feckn delicious shit, oh no..
  */

public class CoffeCup : DroppableInteractable{
    private Int32 m_CupMaterialPattern = ~0x42CAFE43;
    private static int m_CupNumber = 0;
    public List<Material> Materials = new List<Material>();
    private bool m_PickedUp = false;  // Server-Only


    public override void Awake() {
        base.Awake();
        int mat_idx = (m_CupMaterialPattern >> (m_CupNumber++ % 32)) & 1;
        GetComponent<MeshRenderer>().material = Materials[mat_idx];
    }

    public override PlayerAnimation SelfInteraction(PlayerAvatar avatar) {
        avatar.SpeedBoost();
        return PlayerAnimation.DRINK;
    }

    [ServerRpc(RequireOwnership = false)]
    public override void SetHolderServerRpc(int holder_id) {
        base.SetHolderServerRpc(holder_id);
        m_PickedUp = true;
    }

    public bool IsPickedUp() {
        return m_PickedUp;
    }
}
