using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerControllerRB : MonoBehaviour
{
    [Header("Diverse")]
   public Transform spawnPoint;
   public TimerManager timerManager;
    Animator animator;
    AnimatorStateInfo previousState;
    bool isReturning = false;
    Camera mainCamera;

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

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main; // Vi skal bruge det til at rotere spilleren i forhold til kameraet.
        recordDuration = timerManager.maxTime; // Er det her den rigtige måde at gøre det på? Tænkte hvis tiden sku ændres midt kamp etc. så sku vi måske have en anden måde at sætte 
    }
    void OnEnable() => controls.Player.Enable(); //Gode avaner at have, så vi ikke glemmer at enable og disable vores controls.
    void OnDisable() => controls.Player.Disable(); 


    void FixedUpdate() // Bruger FixedUpdate for at matche frameraten på rigidbodiens bevægelse
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);


         if (move.sqrMagnitude > 0.001f) // Rotation.
    {
        if (!isReturning) // Only rotate toward movement if NOT attacking
        {
            Quaternion moveRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, Time.deltaTime * 15f);
        }
    }

         var data = new PlayerFrameData
        {
            position = transform.position,
            rotation = transform.rotation,
            didAttack = false
        };
        recording.Add(data);
    }

    void Update()
    {
        
       
       AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (controls.Player.Attack.triggered && !isReturning && !currentState.IsName("Attack"))
        {
                previousState = currentState;
                Debug.Log("Attack triggered Update");
                animator.SetTrigger("Attack");
                isReturning = true;
        }
        int maxFrames = Mathf.CeilToInt(recordDuration / Time.deltaTime);
        if (recording.Count > maxFrames)
            recording.RemoveAt(0);
            if (isReturning)
                    {
                        

                        if (currentState.IsName("Attack") && currentState.normalizedTime >= 1.0f)
                        {
                            // Return to previous state manually
                            animator.Play(previousState.fullPathHash, 0, 0);
                            isReturning = false;
                        }
                    }
    }

    void Attack()
    {
        RotateTowardMouse();
        Debug.Log("Attack triggered!");
        if (recording.Count > 0)
            recording[recording.Count - 1].didAttack = true;
    }

    // OnTriggerEnter
    // if other.tag == "Damageable"
    // other.tag.GetComponent<Damageable>().TakeDamage(1); // Kan være en god ide at lave en Damageable script som kan bruges til at tage damage fra spilleren.
    void RotateTowardMouse()
    {
    Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

    Plane groundPlane = new Plane(Vector3.up, transform.position);
    float enter;

    if (groundPlane.Raycast(ray, out enter))
{
    Vector3 hitPoint = ray.GetPoint(enter);
    Vector3 direction = hitPoint - transform.position;
    direction.y = 0f; // Ignore vertical difference

    if (direction.sqrMagnitude > 0.001f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
}
    
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

    // Spawner spøgelset på samme sted som spawnpointet.
    GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
    GhostReplayer replayer = ghost.GetComponent<GhostReplayer>();
    replayer.playbackData = new List<PlayerFrameData>(recording);

    recording.Clear(); 
}
}

