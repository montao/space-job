using System.Collections;
using UnityEngine;

public enum Event {
    NONE,
    POWER_OUTAGE,
    COSMIC_HORROR,
    HULL_BREACH,
}

namespace EventParameters {
    public enum HullBreachSize {
        SMALL,
        LARGE,
    }
}

public class EventManager : MonoBehaviour {
    public static EventManager Instance;
    [SerializeField]
    private Map m_Map;
    private float risk = 0.02f * 0.3f;

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
        StartCoroutine(DiceRollCorutine());
    }

    IEnumerator DiceRollCorutine() {
        //Debug.Log("Start Corutine");
        while(true){
            DiceRoll();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public float CurrentRisk() {
        return m_Map.GetState(ShipManager.Instance.GetShipPosition()).risk;
    }

    public void DiceRoll(){
        Debug.Log("Make Diceroll");
        var ship_pos = ShipManager.Instance.GetShipPosition();
        MapState state = m_Map.GetState(ship_pos);
        if(Vector2.Distance(m_LastSpaceEventCoords, ship_pos) > 1 && state.spaceEvent == Event.POWER_OUTAGE){
            ShipManager.Instance.TriggerPowerOutageEvent();
            m_LastSpaceEventCoords = ship_pos;
        }
        float hull_breach_dice = Random.value;
        float fire_dice = Random.value;

        float ship_speed = ShipManager.Instance.GetShipSpeed()/ShipSteering.MAX_TRANSLATION_VELOCITY;
        float hull_breach_risk = (0.3f*Mathf.Atan(4.3f*(ship_speed-0.4f))+0.5f);
        hull_breach_risk = risk * hull_breach_risk * CurrentRisk();

        float fire_risk = (0.5f*Mathf.Atan(4.3f*(ship_speed-0.4f))+0.5f);
        fire_risk = risk * fire_risk * m_Map.GetState(ShipManager.Instance.GetShipPosition()).risk;

        if (hull_breach_dice < hull_breach_risk) {
            ShipManager.Instance.TriggerHullBreachEvent(EventParameters.HullBreachSize.SMALL);
        }
        if (fire_dice < fire_risk) {
            Debug.Log("Fire Risk Peak");
            ShipManager.Instance.TriggerFireEvent();
        }
    }
}
