using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerControllerRB : MonoBehaviour
{
   public Transform spawnPoint;
   public TimerManager timerManager;
   
   
   [Header("Movement of Player")]
    
    [Tooltip("Movement speed of the player")]public float moveSpeed = 5f;
    private Vector2 moveInput;
    
    private Rigidbody rb;
    private InputSystem_Actions controls;
    

     private float recordDuration;

    [Header("Ghost Recording")]
    public List<PlayerFrameData> recording = new List<PlayerFrameData>(); // Behøver den her at være public?
    
    public GameObject ghostPrefab;
    void Awake()
    {
        if (timerManager != null)
        {
            timerManager.OnTimerEnd += SpawnGhost; //Hvordan kan den her køre, siden det er i Awake, lytter Actions hele tiden?
        }
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();

        
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Attack.performed += ctx => Attack();
    }

    // Rotation

    // OnTriggerEnter til Angrebet 
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

         var data = new PlayerFrameData
        {
            position = transform.position,
            rotation = transform.rotation,
            didAttack = false
        };
        recording.Add(data);
        // VEctor3 LookAt
    }

    void Update()
    {
        
       
       
        int maxFrames = Mathf.CeilToInt(recordDuration / Time.deltaTime);
        if (recording.Count > maxFrames)
            recording.RemoveAt(0);

        
    }

    void Attack()
    {
        Debug.Log("Player Attack!");

        
        if (recording.Count > 0)
            recording[recording.Count - 1].didAttack = true;

       
    }

    void SpawnGhost()
{
    if (spawnPoint != null)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero; // SKal man bruge Linær velocity her?
    }

    // SPawner spøgelset på samme sted som spawnpointet.
    GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
    GhostReplayer replayer = ghost.GetComponent<GhostReplayer>();
    replayer.playbackData = new List<PlayerFrameData>(recording);

    recording.Clear(); 
}
}

