using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Camera Settings")]

    public CinemachineCamera vCam;
    public Transform zoomOutTarget;
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

    [Header("Audio Clips")]

   
    public AudioSource musicBattle;
    public AudioSource musicMenu;
    public AudioSource audioStartRobot;

    [Header("Audio Settings")]

    public float fadeMusicDuration = 1f;



    [Header("Animation Settings")]
    public Animator playerAnimator;
    


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

        if (t >= 1f)
        {
            transitioning = false;
            

            // After the animation, assign follow target
            vCam.Follow = player;
            vCam.LookAt = player;
            var transposer = vCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow;
            if (transposer == null)
            {
                transposer = vCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFollow;
                if (transposer == null)
                {
                    transposer = vCam.gameObject.AddComponent<CinemachineFollow>();
                }
            }

            var hardLookAt = vCam.GetCinemachineComponent(CinemachineCore.Stage.Aim) as CinemachineHardLookAt;
            if (hardLookAt == null)
            {
                hardLookAt = vCam.gameObject.AddComponent<CinemachineHardLookAt>();
            }
            

           
        }
    }

    // End Camera Settings
    //

        public void OnTurnOnButtonPressed()
        {
            StartCoroutine(LoadStartScreen());
            TurnOnHasBeenPressed = true;
            
            
        }



    // Channel 1 til Default
    // Channel 2 til at zoome ind på robotten

    IEnumerator LoadStartScreen()
    {
        audioStartRobot.Play();
        float startVolume = musicMenu.volume;

        float t = 0;
        while (t < fadeMusicDuration)
        {
            t += Time.deltaTime;
            musicMenu.volume = Mathf.Lerp(startVolume, 0, t / fadeMusicDuration);
            yield return null;
        }

        musicMenu.Stop();
        
        yield return new WaitForSeconds(2); // Venter på at opstartslyden stopper
        StartZoomOut();

        yield return new WaitForSeconds(2); // Imens vi zoomer ud spiller animationen
        playerAnimator.SetTrigger("TurningOn"); // Trigger animationen
    }


    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    
}
