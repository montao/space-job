using UnityEngine;

public struct MapState {
    public float risk; // red
    public Event spaceEvent; //represented green
    // TODO Biomes (blue)

}

public class Map : MonoBehaviour {
    public static int MIN = 0;
    public static int MAX = 1024;
    public static float DANGER_THRESHOLD = 0.5f;

    public Texture2D MapTexture;
    public Texture2D IngameMapTexture;
    public Texture2D DangerTexture;

    [SerializeField]
    private MapCam m_MapCam;

    void Start() {
        GenerateMap();
    }

    public MapState GetState(Vector2 pos) {
        MapState state = new MapState();
        if (pos.x < MIN || pos.y < MIN || pos.x > MAX || pos.y > MAX) {
            state.spaceEvent = Event.COSMIC_HORROR;
            state.risk = 1;
        } else {
            int y = Mathf.RoundToInt(pos.y);
            if (!SystemInfo.graphicsUVStartsAtTop) {
                y = MAX - y;
            }
            Color color = MapTexture.GetPixel(Mathf.RoundToInt(pos.x), y);
            if(color.g >= DANGER_THRESHOLD){
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
                float risk = (int)(state.risk*16)/16.0f;
                Color col = new Color(risk * 0.9f, 0.2f, 0.2f);

                if (state.risk > DANGER_THRESHOLD) {
                    var dangersize = DangerTexture.width;
                    Color danger_col = DangerTexture.GetPixel(x % dangersize, -y % dangersize);
                    col.r = Mathf.Clamp01(col.r + (danger_col.r * danger_col.a));
                }

                IngameMapTexture.SetPixel(x, y, col);
            }
        }
        IngameMapTexture.Apply();
    }

    public void DropBreadcrumb(Vector2 pos) {
        m_MapCam.DropBreadcrumb(pos);
    }
}
