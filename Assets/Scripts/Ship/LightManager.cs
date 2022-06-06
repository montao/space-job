using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    public List<Light> NormalLights;
    public List<Light> BackupLights;
    public static LightManager Instance;

    private List<LightmapData> m_LightmapData;
    private List<LightmapData> m_EmptyLightmapData;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        m_LightmapData = new List<LightmapData>(LightmapSettings.lightmaps);
        m_EmptyLightmapData = new List<LightmapData>(m_LightmapData.Count);
    }

    void Start() {
        SetNormal(true);
        SetBackup(false);
    }

    public void SetNormal(bool on){
        foreach (Light light in NormalLights){
            light.gameObject.SetActive(on);
        }
        SetLightmapsEnabled(on);
    }

    public void SetBackup(bool on){
        foreach (Light light in BackupLights){
            light.gameObject.SetActive(on);
        }
        // No need to change lightmap status here, since `SetNormal` will be called too.
    }

    private void SetLightmapsEnabled(bool enabled) {
        LightmapSettings.lightmaps = enabled ? m_LightmapData.ToArray() : m_EmptyLightmapData.ToArray();
    }

}
