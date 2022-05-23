using UnityEngine;

public class NavTerminalConsole : MonoBehaviour {

    [SerializeField]
    private SevenSegmentDisplay m_AngleDisp;
    [SerializeField]
    private SevenSegmentDisplay m_TargetSpeedDisp;
    [SerializeField]
    private SevenSegmentDisplay m_SpeedDisp;

    // Update is called once per frame
    void Update() {
        m_AngleDisp.DisplayNumber(Mathf.Repeat(ShipManager.Instance.GetShipAngle(), 360f), 1);
        m_TargetSpeedDisp.DisplayNumber(ShipManager.Instance.GetTargetShipSpeed(), 1);
        m_SpeedDisp.DisplayNumber(ShipManager.Instance.GetShipSpeed(), 1);
    }
}
