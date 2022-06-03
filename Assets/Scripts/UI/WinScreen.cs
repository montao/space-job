using UnityEngine;

public class WinScreen : MonoBehaviour {
    public void OnButtonPress() {
        ShipManager.Instance.StartNewGameServerRpc();
    }
}
