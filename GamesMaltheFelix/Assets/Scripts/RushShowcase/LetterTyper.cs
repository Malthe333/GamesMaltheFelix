using System.Collections;
using UnityEngine;

public class LetterTyper : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textField;
    [SerializeField] private float waitBetweenLetters = 0.15f;
    [SerializeField] private GameObject[] objectsToReveal;
    [SerializeField] private float waitBetweenReveals = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WriteSentence());
    }

    private IEnumerator WriteSentence()
    {
        string sentence = textField.text;
        textField.text = "";

        foreach (GameObject objectToReveal in objectsToReveal)
        {
            objectToReveal.SetActive(false);
        }

        foreach (char c in sentence)
        {
            yield return new WaitForSeconds(waitBetweenLetters);
            textField.text += c;
        }

        foreach (GameObject objectToReveal in objectsToReveal)
        {
            yield return new WaitForSeconds(waitBetweenReveals);
            objectToReveal.SetActive(true);
        }
    }
}
