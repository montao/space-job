using System.Collections;
using UnityEngine;

public enum Event {
    NONE,
    POWER_OUTAGE,
    COSMIC_HORROR,
    HULL_BREACH,
    SYSTEM_FAILURE,
}

namespace EventParameters {
    public enum HullBreachSize {
        SMALL,
        LARGE,
    }
}

public class EventManager : MonoBehaviour {
    public static EventManager Instance;

    public string DiceRollDebugInfo = "";
    [SerializeField]
    private Map m_Map;
    private float risk = 0.2f;

    private Vector2 m_LastSpaceEventCoords = new Vector2(1, 1) * -10000;

    public GameObject HullBreachPrefab;
    public GameObject FirePrefab;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    // Called by ShipManager, and only if current user is hosting
    public void StartDiceRollCoroutine() {
        Debug.Log("Lets Start Event Corutine");
        StartCoroutine(DiceRollCorutine());
    }

    IEnumerator DiceRollCorutine() {
        //Debug.Log("Start Corutine");
        while(true){
            DiceRoll();
            yield return new WaitForSeconds(1f);
        }
    }

    public float CurrentRisk() {
        return m_Map.GetState(ShipManager.Instance.GetShipPosition()).risk;
    }

    public void DiceRoll(){

        DiceRollDebugInfo = "";
        DiceRollDebugInfo += "t = " + Time.fixedTime + "\n";

        Debug.Log("Make Diceroll");
        var ship_pos = ShipManager.Instance.GetShipPosition();
        MapState state = m_Map.GetState(ship_pos);

        float hull_breach_dice = Random.value;
        float fire_dice = Random.value;
        float power_dice = Random.value;
        float sysfail_dice = Random.value;

        float ship_speed = Mathf.Abs(ShipManager.Instance.GetShipSpeed())/ShipSteering.MAX_TRANSLATION_VELOCITY;
        float speed_risk = 0.03f + (ship_speed * 0.85f);
        float map_risk = CurrentRisk();

        float hull_breach_risk = risk * speed_risk * map_risk;
        float fire_risk = hull_breach_risk;
        float power_risk = 0.5f * hull_breach_risk;
        float sysfail_risk = (map_risk > Map.DANGER_THRESHOLD ? 0.2f : 0.00001f);

        DiceRollDebugInfo += "speed_risk = " + speed_risk + "\n";
        DiceRollDebugInfo += "fire_risk = " + fire_risk + "\n";
        DiceRollDebugInfo += "hull_risk = " + hull_breach_risk + "\n";
        DiceRollDebugInfo += "power_risk = " + power_risk + "\n";
        DiceRollDebugInfo += "sysfail_risk = " + sysfail_risk + "\t" + sysfail_dice + "\n";

        if (power_dice < power_risk) {
            ShipManager.Instance.TriggerPowerOutageEvent();
            DiceRollDebugInfo += "POWER OUTAGE ";
        }
        if (hull_breach_dice < hull_breach_risk) {
            Debug.Log("Hull Risk Peak");
            ShipManager.Instance.TriggerHullBreachEvent(EventParameters.HullBreachSize.SMALL);
            DiceRollDebugInfo += "HULL BREACH ";
        }
        if (fire_dice < fire_risk) {
            Debug.Log("Fire Risk Peak");
            ShipManager.Instance.TriggerFireEvent();
            DiceRollDebugInfo += "FIRE ";
        }
        if (sysfail_dice < sysfail_risk && ShipManager.Instance.HasPower) {
            ShipManager.Instance.TriggerSystemFailureEvent();
            DiceRollDebugInfo += "SYSFAIL ";
        }
    }
}
