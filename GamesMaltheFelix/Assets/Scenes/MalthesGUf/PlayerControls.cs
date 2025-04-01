using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerControllerRB : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    public Transform spawnPoint;

    private Rigidbody rb;
    private InputSystem_Actions controls;

    [Header("Ghost Recording")]
    public List<PlayerFrameData> recording = new List<PlayerFrameData>();
    
    public GameObject ghostPrefab;

    public TimerManager timerManager;
    private float recordDuration;
    void Awake()
    {
        if (timerManager != null)
        {
            timerManager.OnTimerEnd += SpawnGhost;
        }
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Attack.performed += ctx => Attack();
    }


    void Start()
    {
        recordDuration = timerManager.maxTime; // Er det her den rigtige måde at gøre det på? Tænkte hvis tiden sku ændres midt kamp etc. så sku vi måske have en anden måde at sætte 
    }
    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        
        // RECORD player movement and actions
        var data = new PlayerFrameData
        {
            position = transform.position,
            rotation = transform.rotation,
            didAttack = false
        };
        recording.Add(data);

        int maxFrames = Mathf.CeilToInt(recordDuration / Time.deltaTime);
        if (recording.Count > maxFrames)
            recording.RemoveAt(0);

        
    }

    void Attack()
    {
        Debug.Log("Player Attack!");

        // Mark last recorded frame as attack
        if (recording.Count > 0)
            recording[recording.Count - 1].didAttack = true;

        // Do your attack logic here (animation, damage, etc.)
    }

    void SpawnGhost()
{
    // Move player back to spawn point
    if (spawnPoint != null)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        // Stop any lingering velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero; // SKal man bruge Linær velocity her?
    }

    // Spawn ghost at the same position
    GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
    GhostReplayer replayer = ghost.GetComponent<GhostReplayer>();
    replayer.playbackData = new List<PlayerFrameData>(recording);

    recording.Clear(); // Fresh recording for the next round
}
}

