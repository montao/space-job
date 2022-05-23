using UnityEngine;

public struct MapState {
    public float risk; // red
    public Event spaceEvent; //represented green
    // TODO Biomes (blue)

}

public class Map : MonoBehaviour {
    public Texture2D MapTexture;
    public Texture2D IngameMapTexture;

    [SerializeField]
    private MapCam m_MapCam;

    void Start() {
        GenerateMap();
    }

    public MapState GetState(Vector2 pos) {
        MapState state = new MapState();
        if (pos.x < 0 || pos.y < 0 || pos.x > 1024 || pos.y > 1024) {
            state.spaceEvent = Event.COSMIC_HORROR;
            state.risk = 1;
        } else {
            Color color = MapTexture.GetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            if(color.g >= 0.5){
                state.spaceEvent = Event.POWER_OUTAGE;
            }
            state.risk = color.r;
        }
        return state;
    }

    public void GenerateMap() {
        for (int x = 0; x < IngameMapTexture.width; ++x) {
            for (int y = 0; y < IngameMapTexture.width; ++y) {
                var state = GetState(new Vector2(x, y));
                float risk = (int)(state.risk*8)/8.0f;
                Color col = new Color(0.2f, risk, 0.2f);
                IngameMapTexture.SetPixel(x, y, col);
            }
        }
        IngameMapTexture.Apply();
    }

    public void DropBreadcrumb(Vector2 pos) {
        m_MapCam.DropBreadcrumb(pos);
    }
}
