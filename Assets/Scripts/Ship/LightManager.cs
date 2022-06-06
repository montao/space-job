using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    public List<Light> NormalLights;
    public List<Light> BackupLights;

    private List<Lamp> m_NormalLamps = new List<Lamp>();
    private List<Lamp> m_BackupLamps = new List<Lamp>();

    public static LightManager Instance;

    private List<LightmapData> m_LightmapData;
    private List<LightmapData> m_EmptyLightmapData;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        foreach (Lamp l in GameObject.FindObjectsOfType<Lamp>()) {
            if (l.CompareTag("Normal Lamp")) {
                m_NormalLamps.Add(l);
            } else if (l.CompareTag("Backup Lamp")) {
                m_BackupLamps.Add(l);
            } else {
                Debug.LogWarning("Untagged Lamp " + l );
            }
        }

        m_LightmapData = new List<LightmapData>(LightmapSettings.lightmaps);
        m_EmptyLightmapData = new List<LightmapData>(m_LightmapData.Count);

        Debug.Log("Lightmanager got: " + m_NormalLamps.Count);
    }

    void Start() {
        SetNormal(true);
        SetBackup(false);
    }

    public void SetNormal(bool on){
        foreach (Light light in NormalLights){
            light.enabled = on;
        }
        foreach (Lamp lamp in m_NormalLamps) {
            lamp.SetOn(on);
        }
        SetLightmapsEnabled(on);
    }

    public void SetBackup(bool on){
        foreach (Light light in BackupLights){
            light.enabled = on;
        }
        foreach (Lamp lamp in m_BackupLamps) {
            lamp.SetOn(on);
        }
        // No need to change lightmap status here, since `SetNormal` will be called too.
    }

    private void SetLightmapsEnabled(bool enabled) {
        LightmapSettings.lightmaps = enabled ? m_LightmapData.ToArray() : m_EmptyLightmapData.ToArray();
    }

}
