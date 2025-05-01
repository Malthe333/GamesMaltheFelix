using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    [Header("Camera Settings")]

    
    public CameraZoomIn cameraZoomIn; // Reference to the CameraZoomIn script
    public CinemachineCamera vCam;
    public Transform zoomOutTarget;

    public CinemachineBrain brain;

    public OutputChannels outputChannels;
    public Transform player;
    public float transitionDuration = 2f;

    bool TurnOnHasBeenPressed = false;

    public float rotationSpeed = 20f;

    public Transform PlayerRotationtarget;

    public Transform CameraRotationTarget;

    private bool transitioning = false;
    private float timer = 0f;
    private Vector3 startPos;
    private Quaternion startRot;


    // 3 Kamera indstillinger   
    //public transform playerTargetZoomIn;  // The target to move toward
    public float moveDuration = 2f;       // Duration of the movement in seconds



    [Header("Audio Clips")]

    public AudioSource musicBattle;
    public AudioSource musicMenu;
    public AudioSource audioStartRobot;

    [Header("Audio Settings")]
    public float fadeMusicDuration = 1f;

    [Header("Animation Settings")]
    public Animator playerAnimator;

    [Header("Events")]

    [SerializeField] private UnityEvent gameStartEvent; 

    [Header("UI Components")]

    public GameObject menuUI;

    public GameObject timerUI;

    public CanvasGroup MenuUIFadeGroup; // Reference to the CanvasGroup component for fading out the menu UI

    private float fadeDurationMenu = 1f; // Duration of the fade-out effect


    


    // Audio Clips Section
    
    void Start()
    {
        musicMenu.Play();
    }
    
    
    
    
    
    
    // Camera settings

     public void StartZoomOut()
    {
        transitioning = true;
        timer = 0f;
        startPos = vCam.transform.position;
        startRot = vCam.transform.rotation;

        // Disable follow/lookat while transitioning
        vCam.Follow = null;
        //vCam.LookAt = null;
    }

    
    void Update()
    {
        
        // Rotate the camera around the player
        if (TurnOnHasBeenPressed == false)
        {
            CameraRotationTarget.RotateAround(PlayerRotationtarget.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
        
        
        
        // Camera settings
        if (!transitioning) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / transitionDuration);

        vCam.transform.position = Vector3.Lerp(startPos, zoomOutTarget.position, t);
        vCam.transform.rotation = Quaternion.Slerp(startRot, zoomOutTarget.rotation, t);


    }

    // End Camera Settings
    //

        public void OnTurnOnButtonPressed()
        {
            StartCoroutine(LoadStartScreen());
            StartCoroutine(FadeOutMenuUI()); // Fade out menu UI
            TurnOnHasBeenPressed = true;
            
            
        }



    // Channel 1 til Default
    // Channel 2 til at zoome ind på robotten



    IEnumerator FadeOutMenuUI()
    {
        float t = 0;
        while (t < fadeDurationMenu)
        {
            t += Time.deltaTime;
            MenuUIFadeGroup.alpha = Mathf.Lerp(1, 0, t / fadeDurationMenu);
            yield return null;
        }

        menuUI.SetActive(false); // Deaktiverer menu UI'en efter fade-out
    }


    IEnumerator LoadStartScreen()
    {
        audioStartRobot.Play();
        cameraZoomIn.StartTheMovement(); // Kalder funktionen til at zoome ind på robotten
        brain.ChannelMask = (OutputChannels)4;// Skifter til kamera som zoomer ind på robotten
        
        
        float startVolume = musicMenu.volume;
        
        float t = 0;
        while (t < fadeMusicDuration)
        {
            t += Time.deltaTime;
            musicMenu.volume = Mathf.Lerp(startVolume, 0, t / fadeMusicDuration);
            yield return null;
        }

        musicMenu.Stop();
        
        yield return new WaitForSeconds(5); // Venter på at opstartslyden stopper
       
        brain.ChannelMask = (OutputChannels)1; // Skifter til kamera i luften



        yield return new WaitForSeconds(2); // Imens vi zoomer ud spiller animationen

        
        // playerAnimator.SetTrigger("TurningOn"); // Trigger animationen
        // TimerManager.hasStarted = true; // Starter timeren
        // timerUI.SetActive(true); // Aktiverer timeren
        // musicBattle.Play(); // Spiller battle musikken
        gameStartEvent?.Invoke(); // Kalder eventet for at starte spillet







    }


    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    
}
