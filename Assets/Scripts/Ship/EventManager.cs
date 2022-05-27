using System.Collections;
using UnityEngine;

public enum Event {
    NONE,
    POWER_OUTAGE,
    COSMIC_HORROR,
    HULL_BREACH,
}

public class EventManager : MonoBehaviour{
    public static EventManager Instance;
    [SerializeField]
    private Map m_Map;
    [SerializeField]
    [Range(0,1)]
    private float risk;

    private Vector2 m_LastSpaceEventCoords = new Vector2(1, 1) * -10000;

    private void Start() {
        StartCoroutine(DiceRollCorutine());
    }

    IEnumerator DiceRollCorutine() {
        while(true){
            DiceRoll();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DiceRoll(){
        var ship_pos = ShipManager.Instance.GetShipPosition();
        MapState state = m_Map.GetState(ship_pos);
        if(Vector2.Distance(m_LastSpaceEventCoords, ship_pos) > 1 && state.spaceEvent == Event.POWER_OUTAGE){
            ShipManager.Instance.TriggerPowerOutageEvent();
            m_LastSpaceEventCoords = ship_pos;
        }        
        
        float hull_breach_dice = Random.value;
        float ship_speed = ShipManager.Instance.GetShipSpeed()/ShipSteering.MAX_TRANSLATION_VELOCITY;
        float hull_breach_risk = (0.3f*Mathf.Atan(4.3f*(ship_speed-0.4f))+0.5f);
        hull_breach_risk = risk * hull_breach_risk * m_Map.GetState(ShipManager.Instance.GetShipPosition()).risk;
        //Debug.Log("ship_speed: " + ship_speed + ", risk: " + m_Map.GetState(ShipManager.Instance.GetShipPosition()).risk + ", riskybiskuit: " + hull_breach_risk);
    }
}
