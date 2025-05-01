using UnityEngine;
using TMPro;

public class WriteVersionNum : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = Application.version;
    }
}
