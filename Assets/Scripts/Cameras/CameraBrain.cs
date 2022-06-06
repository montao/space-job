using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class CameraBrain : MonoBehaviour {

    [SerializeField]
    private ScriptableRendererFeature m_WireframeRendererFeature;
    [SerializeField]
    private Material m_SkyboxWireframe;
    [SerializeField]
    private Material m_SkyboxDefault;

    private Skybox m_Skybox;
    private bool m_IsBlending;
    private bool m_WireframeOnBlend = true;
    public bool WireframeOnBlend {
        get { return m_WireframeOnBlend; }
        set { m_WireframeOnBlend = value; }
    }

    public static CameraBrain Instance;
    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        m_Skybox = GetComponent<Skybox>();
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

    private void SetWireframe(bool wireframe_on) {
        m_WireframeRendererFeature.SetActive(wireframe_on);
        RenderSettings.skybox = wireframe_on ? m_SkyboxWireframe : m_SkyboxDefault;
    }

    void Update() {
        bool blending = Brain.IsBlending;
        if (blending != m_IsBlending) {
            if (m_WireframeOnBlend || !blending) {  // wireframe enabled, or we've just stopped blending
                SetWireframe(blending);
            }
            m_IsBlending = blending;
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            RaycastHit hit;
            var ray = OutputCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.transform.name);
            }
        }
    }

}
