using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PlayerControllerRB : MonoBehaviour
{
    [Header("Diverse")]
    public Transform spawnPoint;
    public TimerManager timerManager;
    [SerializeField, Tooltip("Animator of player")] private Animator playerAnimator;
    bool isReturning = false;
    Camera mainCamera;
    [Tooltip("This is used to calculate movement in accordance to the camera rotation")] private Transform mainCamTrans;

    [Header("Movement of Player")]
    [SerializeField, Tooltip("Movement speed of the player")] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;
    private InputSystem_Actions controls;

    private List<PlayerFrameData> recording = new List<PlayerFrameData>(); // Behøver den her at være public?
    [Header("Ghost Recording")]
    [SerializeField] private GameObject ghostPrefab;




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
        if (playerAnimator == null)
        {
            if (!TryGetComponent(out playerAnimator))
                Debug.LogWarning($"Player {name} had no animator assigned and could not find any either. It will not function correctly");
        }
        
        mainCamera = Camera.main; // Vi skal bruge det til at rotere spilleren i forhold til kameraet.
        mainCamTrans = mainCamera.transform;
    }

    void OnEnable() => controls.Player.Enable(); //Gode avaner at have, så vi ikke glemmer at enable og disable vores controls.
    void OnDisable() => controls.Player.Disable(); 

    void FixedUpdate() // Bruger FixedUpdate for at matche frameraten på rigidbodiens bevægelse
    {
        AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);

        // Make movement direction in accordance with camera look direction
        Vector3 move = CamDirectionTransformation(moveInput);

        rb.linearVelocity = move * moveSpeed; 


         if (move.sqrMagnitude > 0.001f) // Rotation.
         {
            if (!isReturning && !currentState.IsName("Attack")) // Only rotate toward movement if NOT attacking
            {
                Quaternion moveRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, Time.deltaTime * 15f);
            }
         }

         if (!isReturning)
         {
            var data = new PlayerFrameData
            {
                position = transform.position,
                rotation = transform.rotation,
                didAttack = false
            };
            recording.Add(data); // Tilføjer data til listen af frames, så vi kan bruge det til at afspille spøgelset senere.
         }
    }

    /// <summary>
    /// Translates a Vector2 to a Vector3, with the y of the Vector2 pointing in the main cameras forward direction, and the x pointing in the main cameras right direction.
    /// </summary>
    /// <param name="moveDir">The movement input direction</param>
    /// <returns>Normalized Vector3(moveDir.y * camForward, 0f, moveDir.x * camRight)</returns>
    private Vector3 CamDirectionTransformation(Vector2 moveDir)
    {
        Vector3 camForwad = new Vector3(mainCamTrans.forward.x, 0f, mainCamTrans.forward.z).normalized;
        Vector3 camRight = new Vector3(mainCamTrans.right.x, 0f, mainCamTrans.right.z).normalized;
        Vector3 translatedMoveDir = moveDir.y * camForwad + moveDir.x * camRight;
        return translatedMoveDir.normalized;
    }

    void Attack()
    {
        
        AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (isReturning || currentState.IsName("Attack")) return; // Prevents re-triggering the attack animation while rewinding
        
        playerAnimator.SetTrigger("Attack");
        
        //int maxFrames = Mathf.CeilToInt(recordDuration / Time.deltaTime);
        // if (recording.Count > maxFrames)
        //     recording.RemoveAt(0);
        //     if (isReturning)
        //             {
                        

        //                 if (currentState.IsName("Attack") && currentState.normalizedTime >= 1.0f)
        //                 {
        //                     // Return to previous state manually
        //                     animator.Play(previousState.fullPathHash, 0, 0);
        //                     isReturning = false;
        //                 }
        //             }
        RotateTowardMouse();
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
        List<PlayerFrameData> rewindData = new List<PlayerFrameData>(recording);
        if (spawnPoint != null)
        {
            // transform.position = spawnPoint.position;
            // transform.rotation = spawnPoint.rotation;
            recording.Clear(); // Uncomment den her hvis det ik virker
        
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero; // SKal man bruge Linær velocity her?
        }

        // Spawner spøgelset på samme sted som spawnpointet.
        /*
        GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
        GhostReplayer replayer = ghost.GetComponent<GhostReplayer>();
        replayer.playbackData = rewindData.Select(frame => new PlayerFrameData
        {
            position = frame.position,
            rotation = frame.rotation,
            didAttack = frame.didAttack
        }).ToList();
        */

    
        StartCoroutine(RewindPlayer(rewindData));
    }

    IEnumerator RewindPlayer(List<PlayerFrameData> rewindData)
    {
        isReturning = true;
        timerManager?.PauseTimer(); // Pauser timeren når vi spoler tilbage så vi ikke optager mens vi spoler tilbage.

        int frameCount = rewindData.Count;
        if (frameCount == 0)
        {
            isReturning = false;
            yield break;
        }

        float rewindDuration = 0.5f;
        float elapsed = 0f;

        // Disable control and physics during rewind
        controls.Player.Disable();
        rb.isKinematic = true;

        // Make player end all current animations, quick fix by just speeding through them
        playerAnimator.speed = 99f;

        while (elapsed < rewindDuration)
        {
            float t = elapsed / rewindDuration;
            // Convert t from [0,1] into an index from end to start
            float rawIndex = Mathf.Lerp(frameCount - 1, 0, t);
            int lowerIndex = Mathf.FloorToInt(rawIndex);
            int upperIndex = Mathf.Clamp(lowerIndex + 1, 0, frameCount - 1);
            float lerpT = rawIndex - lowerIndex;

            Vector3 pos = Vector3.Lerp(rewindData[lowerIndex].position, rewindData[upperIndex].position, lerpT);
            Quaternion rot = Quaternion.Slerp(rewindData[lowerIndex].rotation, rewindData[upperIndex].rotation, lerpT);

            transform.position = pos;
            transform.rotation = rot;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to spawn point in case first frame has teeny tiny offset
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        // Make player animated normally again
        playerAnimator.speed = 1f;

        rb.isKinematic = false;
        controls.Player.Enable();
        isReturning = false;


        timerManager?.ResetTimer(); // Resetter timeren når vi er færdige med at spole tilbage.

        if (ghostPrefab != null && spawnPoint != null)
        {
            GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
            GhostReplayer replayer = ghost.GetComponent<GhostReplayer>();
            replayer.playbackData = new List<PlayerFrameData>(rewindData);
        }
    }
}

