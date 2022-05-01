using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    public List<Light> NormalLights;
    public List<Light> BackupLights;
    public static LightManager Instance;
    public void SetNormal(bool on){
        foreach (Light light in NormalLights){
            light.gameObject.SetActive(on);
        }
    }
    public void SetBackup(bool on){
        foreach (Light light in BackupLights){
            light.gameObject.SetActive(on);
        }
    }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    void Start() {
        SetNormal(true);
        SetBackup(false);
    }
}
