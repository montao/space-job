using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public struct Destination : INetworkSerializeByMemcpy {
    public Vector2 pos;
    public bool reached;
}

public class ShipManager : NetworkBehaviour {
    public static ShipManager Instance;
    private ShipSteering m_Steering;
    public ShipSteering Steering {
        get => m_Steering;
    }

    public static float WIN_DISTANCE_THRESHOLD = 15f;
    public static int DESTINATION_NONE = -1;

    public const char HAS_POWER = '\0';

    public List<Room> Rooms = new List<Room>();

    private NetworkVariable<float> m_Oxygen = new NetworkVariable<float>(1f);
    private NetworkVariable<char> m_Power = new NetworkVariable<char>(HAS_POWER);
    private NetworkVariable<Vector2> m_Position = new NetworkVariable<Vector2>(new Vector2(512f, 512f));
    private NetworkVariable<float> m_Rotation = new NetworkVariable<float>(0f);
    private NetworkVariable<float> m_Speed = new NetworkVariable<float>(0f);
    private NetworkVariable<float> m_Odometer = new NetworkVariable<float>(0f);
    private NetworkVariable<bool> m_Won = new NetworkVariable<bool>(false);
    private NetworkVariable<int> m_DestinationIndex = new NetworkVariable<int>(0);  // might init to DESTINATION_NONE?
    private float m_DistSinceLastBreadcrumb = 0;
    private Map m_Map;
    private float m_DistanceToWin;

    private List<Destination> m_Destinations = new List<Destination>();

    public PowerTerminal PowerTerminal;

    [SerializeField]
    private WinScreen m_WinScreen;

    [SerializeField]
    private Material m_TransitionMaterial;
    private Color m_TransitionColorNormal;
    [SerializeField]
    private Color m_TransitionColorOutage;

    public static char[] ERROR_CODES = {'2', 'e', (char)0xba, (char)42, '\n'};
    [SerializeField]
    private AudioSource audioSourceLamps;
    [SerializeField]
    private AudioClip lampSound;

    public static float POWER_OUTAGE_COOLDOWN = 20f;
    private float m_LastPowerOutage = -100f;

    private Plant[] plants;

    public static string PowerSolutionCode(char error_code) {
        return Convert.ToByte((error_code >> 1) ^ 'a').ToString("x2").ToUpper()
                + Convert.ToByte(error_code | 'B').ToString("x2").ToUpper();
    }
    public static string ErrorCodeDisplay(char power) {
        return Convert.ToByte(power ^ 'L').ToString("x2").ToUpper()
                + Convert.ToByte(power).ToString("x2").ToUpper();
    }

    public bool HasPower{
        get => m_Power.Value == HAS_POWER;
    }

    protected void OnPowerChange(char _, char power){
        bool hasPower = power == HAS_POWER;
        
        if(LightManager.Instance != null){
            LightManager.Instance.SetBackup(!hasPower);
            LightManager.Instance.SetNormal(hasPower);
        }
        

        if(PowerTerminal != null){   
            Debug.Log("Display Error");
            if (!hasPower) {
                PowerTerminal.DisplayError("0x" + ErrorCodeDisplay(power));
            } else {
                PowerTerminal.DisplayError("Power: 100%");
            }
        } 

        if (m_TransitionMaterial != null) {
            var col = hasPower ? m_TransitionColorNormal : m_TransitionColorOutage;
            m_TransitionMaterial.SetColor("_WireColor", col);
        }

    }
    protected void OnWinChange(bool prev, bool current) {
        if (m_WinScreen != null) {
            m_WinScreen.SetEnabled(current);
        }
    }
    public override void OnNetworkSpawn(){
        m_Power.OnValueChanged += OnPowerChange;
        m_Won.OnValueChanged += OnWinChange;
        OnPowerChange(HAS_POWER, HAS_POWER);
        OnWinChange(false, false);

        if (IsServer) {
            // setup destinations for all clients
            var dests = new List<Destination>();
            foreach (var dest_pos in m_Map.Destinations) {
                Destination d = new Destination();
                d.pos = dest_pos;
                d.reached = false;
                dests.Add(d);
            }
            SetDestinationsClientRpc(dests.ToArray());
            NetworkManager.Singleton.OnClientConnectedCallback += (ulong client_id) => {
                SetDestinationsClientRpc(m_Destinations.ToArray());
            };

            // spawn ship at random location
            float risk_thres = 0.08f;
            float distance_goal_thres = 150f;
            // debug: show possible spawn locations
            /*
            for (int x = Map.MIN; x < Map.MAX; ++x) {
                for (int y = Map.MIN; y < Map.MAX; ++y) {
                    if (m_Map.GetState(new Vector2(x, y)).risk < risk_thres) {
                        var pos = new Vector2(x, y);
                        var dist = (pos - GetNearestDestination(pos).pos).magnitude;
                        if (x == 0 && y == 0) {
                            Debug.Log("DIST:  " + dist);
                        }
                        if (dist > distance_goal_thres) {
                            var col = m_Map.IngameMapTexture.GetPixel(x, y);
                            col.b = 1f;
                            m_Map.IngameMapTexture.SetPixel(x, y, col);
                        }
                    }
                }
            }
            m_Map.IngameMapTexture.Apply(); */
            // // //

            int n = 0;
            float risk = 1f;
            float distance_goal = 0f;
            Vector2 spawn = Vector2.zero;
            while (risk > risk_thres || distance_goal < distance_goal_thres) {
                spawn = Util.RandomVec2(Map.MIN, Map.MAX);
                risk = m_Map.GetState(spawn).risk;
                distance_goal = (spawn - GetNearestDestination(spawn).pos).magnitude;
                if (n > 30) {
                    risk_thres += 0.001f;
                    distance_goal_thres -= 1.0f;
                }
                ++n;
            }
            m_Position.Value = spawn;
            Debug.Log("Found ship spawn in " + n + " iterations.");
        }
    }
    public override void OnNetworkDespawn(){
        m_Power.OnValueChanged -= OnPowerChange;
        m_Won.OnValueChanged -= OnWinChange;
    }

    public float GetShipSpeed(){
        return m_Steering.GetSpeed();
    }
    public float GetTargetShipSpeed(){
        return m_Steering.GetTargetSpeed();
    }
    public float GetShipAngle(){
        return m_Rotation.Value;
    }
    public float GetShipAngleSpeed(){
        return m_Steering.GetAngularSpeed();
    }
    public Vector2 GetShipPosition(){
        return m_Position.Value;
    }

    public float GetDistantToWin(){
        return m_DistanceToWin;
    }

    public List<Destination> GetDestinations() {
        return m_Destinations;
    }

    public int GetCurrentDestinationIndex() {
        return m_DestinationIndex.Value;
    }

    public Destination GetCurrentDestination() {
        if (m_DestinationIndex.Value == DESTINATION_NONE) {
            return new Destination();
        }
        return m_Destinations[m_DestinationIndex.Value];
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetDestinationIndexServerRpc(int index) {
        m_DestinationIndex.Value = index;
    }

    public Destination GetNearestDestination() {
        return GetNearestDestination(GetShipPosition());
    }

    public Destination GetNearestDestination(Vector2 to) {
        Destination nearest = new Destination();
        nearest.pos = Vector2.one * -100000;
        foreach (var dest in m_Destinations) {
            if (dest.reached) {
                continue;
            }
            var dist = (to - dest.pos).magnitude;
            var dist_min = (to - nearest.pos).magnitude;
            if (dist < dist_min) {
                nearest = dest;
            }
        }
        return nearest;
    }

    public Vector2 GetGoal(){
        return GetCurrentDestination().pos;
    }

    public void Rotate(float delta_angle) {
        m_Rotation.Value += delta_angle;
    }
    public void Move(Vector2 delta_pos_raw) {
        var new_pos = m_Position.Value + delta_pos_raw;
        new_pos.x = Mathf.Clamp(new_pos.x, Map.MIN, Map.MAX);
        new_pos.y = Mathf.Clamp(new_pos.y, Map.MIN, Map.MAX);

        var delta_pos = new_pos - m_Position.Value;
        m_Position.Value = new_pos;
        m_Odometer.Value += Vector3.Magnitude(delta_pos);
        m_DistSinceLastBreadcrumb += Vector3.Magnitude(delta_pos);

        if (m_DistSinceLastBreadcrumb > Mathf.Lerp(8, 64, GetShipSpeed()/ShipSteering.MAX_TRANSLATION_VELOCITY)) {
            DropBreadcrumbClientRpc(GetShipPosition());
            m_DistSinceLastBreadcrumb = 0;
        }
    }

    [ClientRpc]
    public void DropBreadcrumbClientRpc(Vector2 ship_pos) {
        m_Map.DropBreadcrumb(ship_pos);
    }

    public bool TriggerPowerOutageEvent() {
        if (!HasPower || (m_LastPowerOutage + POWER_OUTAGE_COOLDOWN) >= Time.fixedTime) {
            return false;
        }
        m_LastPowerOutage = Time.fixedTime;
        ShipManager.Instance.Steering.SetTargetVelocityServerRpc(2);
        int error_idx = UnityEngine.Random.Range(0, ERROR_CODES.Length - 1);
        m_Power.Value = ERROR_CODES[error_idx];
        return true;
    }

    public bool TryResolvePowerOutageEvent(string solution_attempt) {
        if (solution_attempt == PowerSolutionCode(m_Power.Value)) {
            ResolvePowerOutageEvent();
            return true;
        } else {
            return false;
        }
    }
    private void ResolvePowerOutageEvent() {
        m_Power.Value = HAS_POWER;
        audioSourceLamps.PlayOneShot(lampSound);
        m_LastPowerOutage = Time.fixedTime;
    }

    public void TriggerHullBreachEvent(EventParameters.HullBreachSize size) {
        Room room = Util.RandomChoice(Rooms);
        if (room != null) {
            room.SpawnHullBreach(size);
        }
    }

    public void TriggerFireEvent() {
        Room room = Util.RandomChoice(Rooms);
        room.SpawnFire();
    }

    public void TriggerSystemFailureEvent() {
        if (!TriggerPowerOutageEvent()) {
            return;
        }
        int n_breaches = UnityEngine.Random.Range(1, 3);
        int n_fires = UnityEngine.Random.Range(1, 7);
        for (int i = 0; i < n_breaches; ++i) {
            TriggerHullBreachEvent(EventParameters.HullBreachSize.SMALL);
        }
        for (int i = 0; i < n_fires; ++i) {
            TriggerFireEvent();
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
        }

        m_Map = GetComponent<Map>();
        m_Steering = GetComponent<ShipSteering>();
        m_TransitionColorNormal = m_TransitionMaterial.GetColor("_WireColor");
    }

    public float GetPlantOxygen(){
        float oxygen = 0.0f;
        plants = FindObjectsOfType<Plant>();
        foreach (Plant p in plants){
            if(p.grown.Value){
                oxygen += 1.0f;
            }
        }
        Debug.Log(oxygen/10);
        return oxygen/10;
    }
    public override void OnDestroy() {
        base.OnDestroy();
        m_TransitionMaterial.SetColor("_WireColor", m_TransitionColorNormal);
    }

    private void Start() {
        if (IsServer) {
            GetComponent<EventManager>().StartDiceRollCoroutine();
        }

        if (!IsServer && !IsClient) {  // not started yeeet
            Debug.Log("Will start dice rolling when server ready");
            NetworkManager.Singleton.OnServerStarted += () => {
                if (IsServer) {
                    GetComponent<EventManager>().StartDiceRollCoroutine();
                }
            };
        }
    }

    private void UpdatePosition() {

        // float angle = m_Rotation.Value*(Mathf.PI/180f);
        // Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // m_Position.Value = m_Position.Value + (direction * m_Speed.Value * delta_time);

        MapState state = m_Map.GetState(m_Position.Value); 
        
    }


    private void CheckWinCondition(){
        m_DistanceToWin = (GetNearestDestination().pos - m_Position.Value).magnitude; 
        if (m_DistanceToWin <= WIN_DISTANCE_THRESHOLD){
            if (IsServer) {
                //m_Won.Value = true;
            }
        } 
    }

    private void Update() {
#if !DISABLE_DEBUG_KEYS
        if(Input.GetKeyDown(KeyCode.P) && IsServer){
            if (HasPower) {
                TriggerPowerOutageEvent();
            } else {
                ResolvePowerOutageEvent();
            }
        }
        if(Input.GetKeyDown(KeyCode.U) && IsServer){
            TriggerHullBreachEvent(EventParameters.HullBreachSize.SMALL);
        }
        if(Input.GetKeyDown(KeyCode.F) && IsServer){
            TriggerFireEvent();
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            Rooms[0].RoomOxygen = 0;
            foreach (Room room in Rooms) {
                Debug.Log(room.Name + ": " + room.RoomOxygen);
            }
        }
#endif
        
        CheckWinCondition();

        if (IsServer) {
#if !DISABLE_DEBUG_KEYS
            if (Input.GetKey(KeyCode.UpArrow)){
                m_Speed.Value += 0.1f;
            }
            if (Input.GetKey(KeyCode.DownArrow)){
                if(m_Speed.Value >= 0f){
                    m_Speed.Value -= 0.1f;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow)){
                m_Rotation.Value += 1f;
            }
            if (Input.GetKey(KeyCode.RightArrow)){
                m_Rotation.Value -= 1f;
            }
            if (Input.GetKeyDown(KeyCode.Y)) {
                MarkNearestDestinationAsReached();
            }
#endif

            UpdatePosition();
            if (GetNearestDestination().pos.magnitude > 5.0f * (Map.MAX - Map.MIN)) {
                // all destinations reached
                ShipManager.Instance.SetWon();
            }
        }
    }

    public void SetWon() {
        m_Won.Value = true;
    }

    [ServerRpc(RequireOwnership=false)]
    public void StartNewGameServerRpc() {

        if (!m_Won.Value) {
            // already called
            return;
        }

        // TODO any more setup we need to do?
        Debug.Log("All players ready, moving to Lobby...");
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= PlayerManager.Instance.StartShip;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PlayerManager.Instance.MovePlayersToSpawns;
        NetworkManager.Singleton.SceneManager.LoadScene("Lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);

        m_Won.Value = false;

        /*
        Steering.ResetSteering();
        Vector2 ship_pos = Util.RandomVec2(256, 1024-256);
        Vector2 destination;
        do {
            destination = Util.RandomVec2(256, 1024-256);
        } while(Vector2.Distance(ship_pos, destination) < 32);
        m_Position.Value = ship_pos;
        m_Rotation.Value = 0;
        */
    }

    public void MarkNearestDestinationAsReached() {
        if (!IsServer) {
            Debug.LogWarning("MarkNearestDestinationAsReached should only be called by server.");
            return;
        }
        var dest = GetNearestDestination();
        if (m_DistanceToWin <= (WIN_DISTANCE_THRESHOLD * 1.02f)) {
            var idx = m_Destinations.IndexOf(dest);
            MarkDestinationReachedClientRpc((uint)idx);
        }
    }

    [ClientRpc]
    public void SetDestinationsClientRpc(Destination[] dests)  {
        Debug.Log("Got " + dests.Length + " destinations!");
        m_Destinations = new List<Destination>(dests);
    }

    [ClientRpc]
    public void MarkDestinationReachedClientRpc(uint idx) {
        if (idx >= m_Destinations.Count) {
            Debug.LogError("Cannot mark dest " + idx + " as reached, index out of bounds.");
            return;
        }
        var dest = m_Destinations[(int)idx];
        dest.reached = true;
        m_Destinations[(int)idx] = dest;
        Debug.Log("Marked destination #" + idx + " as reached.");
    }
}
