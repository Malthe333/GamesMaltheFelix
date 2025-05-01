using UnityEngine;

public class EchoAnimFix : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Animator>().SetBool("IsEcho", true);
    }
}
