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

    public ICinemachineCamera ActiveCamera {
        get {
            if (Brain.ActiveVirtualCamera != null) {
                return Brain.ActiveVirtualCamera;
            } else {
                return null;
            }
        }
    }

    public Camera OutputCamera {
        get => Brain.OutputCamera;
    }

    public GameObject ActiveCameraObject {
        get {
            if (Brain.ActiveVirtualCamera != null) {
                return Brain.ActiveVirtualCamera.VirtualCameraGameObject;
            } else {
                return null;
            }
        }
    }
    public Transform ActiveCameraTransform {
        get {
            GameObject cam = ActiveCameraObject;
            if (cam != null) {
                return ActiveCameraObject.transform;
            } else {
                return gameObject.transform;  // whatever
            }
        }
    }

    public void LookAt(Transform target) {
        Brain.ActiveVirtualCamera.LookAt = target;
    }

}
