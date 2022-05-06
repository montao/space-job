using UnityEngine;

public class Scope : MonoBehaviour {
    public Renderer ScreenRenderer;

    void Update() {
        float dx = Time.time * 0.5f + (Random.value - 0.5f) * 0.03f;
        float dy = 0.03f + (Random.value - 0.5f) * 0.01f;
        ScreenRenderer.material.SetTextureOffset("_BaseMap", new Vector2(dx, dy));
    }
}
