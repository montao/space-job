using UnityEngine;

public class SpaceCamera : MonoBehaviour {

    public const int RENDER_TEXTURE_SCALE_DOWN = 2;

    [SerializeField]
    private Material m_SpaceMaterial;

    private Camera m_Camera;
    private RenderTexture m_RenderTex = null;

    void Awake() {
        m_Camera = GetComponent<Camera>();
        if (!Application.isEditor) {
            CreateRenderTexture();
        }
    }

    void LateUpdate() {
        Quaternion ship_rotation = default;
        if (ShipManager.Instance) {
            var angle = ShipManager.Instance.GetShipAngle();
            ship_rotation = Quaternion.Euler(0, angle, 0);
        }
        transform.rotation = ship_rotation * CameraBrain.Instance.OutputCamera.transform.rotation;
        m_Camera.fieldOfView = CameraBrain.Instance.OutputCamera.fieldOfView;
    }

    public void CreateRenderTexture() {
        if (m_RenderTex != null) {
            // i hope this deletes it?
            m_RenderTex.Release();
            m_RenderTex.DiscardContents();
            m_RenderTex = null;
        }

        int width = Screen.width / RENDER_TEXTURE_SCALE_DOWN;
        int height = Screen.height / RENDER_TEXTURE_SCALE_DOWN;
        m_RenderTex = new RenderTexture(width, height, 0);
        m_Camera.targetTexture = m_RenderTex;
        m_SpaceMaterial.mainTexture = m_RenderTex;
    }
}
