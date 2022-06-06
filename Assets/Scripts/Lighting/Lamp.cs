using UnityEngine;
using System.Collections.Generic;

public class Lamp : MonoBehaviour {
    [SerializeField]
    private MeshRenderer m_BulbRenderer;
    [SerializeField]
    private List<Light> m_DynamicLights;

    public Material BulbGlowMaterial;
    public Material BulbOffMaterial;



    public void SetOn(bool on) {
        foreach (Light light in m_DynamicLights) {
            light.enabled = on;
        }
        m_BulbRenderer.material = on ? BulbGlowMaterial : BulbOffMaterial;
    }
}
