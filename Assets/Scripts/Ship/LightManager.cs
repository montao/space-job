using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    public List<Light> NormalLights;
    public List<Light> BackupLights;
    public static LightManager Instance;

    private LightmapData[] m_LightmapData;
    private LightmapData[] m_EmptyLightmapData;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        m_LightmapData = LightmapSettings.lightmaps;
        m_EmptyLightmapData = new LightmapData[]{};
    }

    void Start() {
        SetNormal(true);
        SetBackup(false);
    }

    public void SetNormal(bool on){
        foreach (Light light in NormalLights){
            light.gameObject.SetActive(on);
        }
        //SetLightmapsEnabled(true);
    }

    public void SetBackup(bool on){
        foreach (Light light in BackupLights){
            light.gameObject.SetActive(on);
        }
        //SetLightmapsEnabled(false);
    }

    private void SetLightmapsEnabled(bool enabled) {
        LightmapSettings.lightmaps = enabled ? m_LightmapData : m_EmptyLightmapData;
    }

}
