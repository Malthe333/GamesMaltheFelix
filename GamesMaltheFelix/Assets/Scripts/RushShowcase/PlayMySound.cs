using UnityEngine;

public class PlayMySound : MonoBehaviour
{
    [SerializeField] private AudioSource[] soundSource;
        
    public void PlaySound(int whichSource)
    {
        soundSource[whichSource].Play();
    }
}
