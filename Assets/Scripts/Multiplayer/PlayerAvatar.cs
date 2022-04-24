using UnityEngine;
using Unity.Netcode;
using TMPro;

[System.Serializable]
public struct PlayerPos {
    public Vector3 Position;
    public Quaternion Rotation;
}

[System.Serializable]
public struct Inventory {
    public ulong primary;
    public ulong secondary;
}

[RequireComponent(typeof(NetworkObject))]
public class PlayerAvatar : NetworkBehaviour {

    private NetworkVariable<PlayerPos> m_playerPos
            = new NetworkVariable<PlayerPos>();
    private NetworkVariable<Inventory> m_inventory
            = new NetworkVariable<Inventory>();

    [ServerRpc]
    public void UpdatePosServerRpc(PlayerPos p) {
        m_playerPos.Value = p;
    }


    private CharacterController m_controller;
    private float m_movementSpeed = 5f;
    private bool m_isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public const float GRAVITY = -10f;  //in case of zero gravity this need to change
    private PersistentPlayer m_localPlayer;
    private Vector3 m_Velocity;

    public TMP_Text nameText;

    public void Start() {
        m_controller = GetComponent<CharacterController>();

        foreach (var player in FindObjectsOfType<PersistentPlayer>()) {
            if (player.OwnerClientId == OwnerClientId) {
                m_localPlayer = player;
                break;
            }
        }
        name = m_localPlayer.PlayerName;
    }

    void Update() {
        if (IsClient) {
            UpdateNameTag();
            if (IsOwner) {
                ProcessInput();
            } else {
                UpdatePos();
            }
        }
    }
    public void PerformGroundCheck() {
        m_isGrounded = Physics.CheckSphere(groundCheck.position,
                GroundCheck.GROUND_CHECK_RADIUS,
                groundLayer
        );
    }
    void UpdateNameTag() {
        nameText.text = m_localPlayer.PlayerName;  // TODO only update when needed?
        nameText.gameObject.transform.LookAt(Camera.main.transform.position);
        nameText.gameObject.transform.Rotate(Vector3.up, 180f);  // mirror
    }

    void ProcessInput() {
        PerformGroundCheck();
        if (m_isGrounded && m_Velocity.y < 0) {
            m_Velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontalInput, 0, verticalInput);

        var cameraDirection = Camera.main.transform.rotation;
        direction = Vector3.Normalize(cameraDirection * direction);
        direction.y = 0;  // no flying allowed!

        m_controller.Move(direction * Time.deltaTime * m_movementSpeed);
        m_controller.transform.LookAt(m_controller.transform.position + direction);

        m_Velocity.y += GRAVITY * Time.deltaTime;
        m_controller.Move(m_Velocity * Time.deltaTime);

        var p = new PlayerPos();
        p.Position = m_controller.transform.position;
        p.Rotation = m_controller.transform.rotation;

        UpdatePosServerRpc(p);
    }

    void UpdatePos() {
        transform.position = m_playerPos.Value.Position;
        transform.rotation = m_playerPos.Value.Rotation;
    }
}
