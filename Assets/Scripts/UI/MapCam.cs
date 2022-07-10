using UnityEngine;
using System.Collections.Generic;

public class MapCam : MonoBehaviour {

    public GameObject BreadcrumbPrefab;
    private Queue<GameObject> m_Breadcrumbs = new Queue<GameObject>();
    public static readonly int MAX_BREADCRUMB_COUNT = 30;

    // (-MaxValue, -MaxValue) => (0, 0) on map,
    // (+MaxValue, +MaxValue) => (1024, 1024) on map,
    private static readonly float m_MaxValue = 5.0f;

    [SerializeField]
    private Transform m_ShipIcon;
    [SerializeField]
    private Transform m_FlagIcon;
    [SerializeField]
    private Transform m_DirectionIcon;
    
    private MeshRenderer m_DirectionIconRenderer;

    void Start() {
        m_DirectionIconRenderer = m_DirectionIcon.gameObject.GetComponentInChildren<MeshRenderer>();
    }

    // map pos -> unity world pos
    private static float Convert(float map_pos) {
        float unclamped = ((map_pos - 512.0f) / 512.0f) * m_MaxValue;
        return Mathf.Clamp(unclamped, -2 * m_MaxValue, 2 * m_MaxValue);
    }
    private static Vector3 Convert3(Vector2 map_pos, float y) {
        float x = Convert(map_pos.x);
        float z = Convert(map_pos.y);
        return new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update() {
        if (!ShipManager.Instance) {
            return;
        }
        Vector2 ship_pos = ShipManager.Instance.GetShipPosition();
        transform.position = Convert3(ship_pos, transform.position.y);

        m_ShipIcon.localRotation = Quaternion.Euler(0, 0, ShipManager.Instance.GetShipAngle());

        UpdateGoal();
    }

    public static float Angle(Vector2 vec) {
        if (vec.y >= 0) return Mathf.Acos(vec.x/vec.magnitude);
        return -Mathf.Acos(vec.x/vec.magnitude);
    }

    void UpdateGoal() {
        Vector2 goal_pos = ShipManager.Instance.GetGoal();
        Debug.Log("GOAL:  " + goal_pos);
        m_FlagIcon.position = Convert3(goal_pos, m_ShipIcon.transform.position.y);

        Vector2 target_direction = (ShipManager.Instance.GetGoal() - ShipManager.Instance.GetShipPosition());

        m_DirectionIconRenderer.enabled = target_direction.magnitude >= 70.0f;

        target_direction.Normalize();
        float angle_rad = Angle(target_direction);
        m_DirectionIcon.localRotation = Quaternion.Euler(0, 0, angle_rad * 180f/Mathf.PI);

    }

    public void DropBreadcrumb(Vector2 pos) {
        GameObject crumb = Instantiate(
                BreadcrumbPrefab,
                Convert3(pos, m_ShipIcon.transform.position.y),
                BreadcrumbPrefab.transform.rotation
        );
        m_Breadcrumbs.Enqueue(crumb);
        if (m_Breadcrumbs.Count > MAX_BREADCRUMB_COUNT) {
            Destroy(m_Breadcrumbs.Dequeue());
        }
    }
}
