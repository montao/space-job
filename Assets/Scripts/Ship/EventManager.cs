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

    public void DiceRoll(){
        MapState state = m_Map.GetState(ShipManager.Instance.GetShipPosition());
        if(state.spaceEvent == Event.POWER_OUTAGE){
            ShipManager.Instance.TriggerPowerOutageEvent();
        }        
        
        float hull_breach_dice = Random.value;

        float hull_breach_risk;

    }
}