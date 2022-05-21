using UnityEngine;

public enum Event {
    NONE,
    POWER_OUTAGE,
    COSMIC_HORROR,
    HULL_BREACH,
}

public class EventManager : MonoBehaviour{
    [SerializeField]
    private Map m_Map;
    [SerializeField]
    [Range(0,1)]
    private float risk;

    public void DiceRoll(){
        MapState state = m_Map.GetState(ShipManager.Instance.GetShipPosition());
        if(state.spaceEvent == Event.POWER_OUTAGE){
            ShipManager.Instance.TriggerPowerOutageEvent();
        }        
        
        float hull_breach_dice = Random.value;
        float ship_speed = ShipManager.Instance.GetShipSpeed()/ShipSteering.MAX_TRANSLATION_VELOCITY;
        float hull_breach_risk = (0.3f*Mathf.Atan(4.3f*(ship_speed-0.4f))+0.5f);
        Debug.Log("ship_speed: " + ship_speed + ", riskybiskuit: " + hull_breach_risk);
    }
}