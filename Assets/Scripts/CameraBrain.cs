using UnityEngine;
using Cinemachine;

public class CameraBrain : MonoBehaviour {

    public static CameraBrain Instance;
    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }


    public CinemachineBrain Brain;

    public GameObject ActiveCameraObject {
        get => Brain.ActiveVirtualCamera.VirtualCameraGameObject;
    }
    public Transform ActiveCameraTransform {
        get => ActiveCameraObject.transform;
    }

    public void LookAt(Transform target) {
        Brain.ActiveVirtualCamera.LookAt = target;
    }

}
