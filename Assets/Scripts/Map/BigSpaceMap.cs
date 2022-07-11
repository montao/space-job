using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpaceMap : TwoLevelInteractable {

    [SerializeField]
    private Transform m_ShipIndicator;
    [SerializeField]
    private Transform m_DestinationButtonsParent;

    private List<DestinationButton> m_DestinationButtons = new List<DestinationButton>();

    public override void Awake() {
        base.Awake();
        var buttons = m_DestinationButtonsParent.GetComponentsInChildren<DestinationButton>();
        m_DestinationButtons = new List<DestinationButton>(buttons);
    }

    // Update is called once per frame
    public override void Update() {
        var y = m_ShipIndicator.localPosition.y;
        var shippos = ShipManager.Instance.GetShipPosition();
        m_ShipIndicator.localPosition = MapCam.Convert3(shippos, y);

        UpdateDestinations();
    }

    public override string FriendlyName() {
        return "Big Space Map";
    }

    void UpdateDestinations() {
        var destinations = ShipManager.Instance.GetDestinations();
        float y = 0.0f;
        for (int i = 0; i < Mathf.Max(destinations.Count, m_DestinationButtons.Count); ++i) {
            if (i >= m_DestinationButtons.Count) {
                Debug.LogWarning("BigSpaceMap: There are not enough DestinationButtons.  Please add more!");
                return;
            }
            var button = m_DestinationButtons[i];
            if (i >= destinations.Count || destinations[i].reached) {
                button.gameObject.SetActive(false);
            } else {
                button.gameObject.SetActive(true);
                var dest = destinations[i];
                var dest_pos_map_local = MapCam.Convert3(dest.pos, y);
                button.transform.localPosition = dest_pos_map_local;
                button.DestinationIndex = i;
            }
        }
    }
}
