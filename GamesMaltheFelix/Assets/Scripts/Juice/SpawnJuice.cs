using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SpawnJuice : MonoBehaviour
{
    [SerializeField] private GameObject juiceEffect;
    [SerializeField] private float juiceLife = 1f;
    [SerializeField] private UnityEvent afterJuiceEvent;

    public void JuiceItUp(bool worldSpace = true)
    {
        GameObject juice;

        if (worldSpace)
        {
            juice = Instantiate(juiceEffect, transform.position, Quaternion.identity);
        }
        else
        {
            juice = Instantiate(juiceEffect, transform);
        }

        StartCoroutine(DrinkUp(juice));
    }

    /// <summary>
    /// Destroys a gameobject. To be used on juice after it has finished playing its effect
    /// </summary>
    /// <param name="juice">The gameobject to be destroyed</param>
    /// <returns></returns>
    private IEnumerator DrinkUp(GameObject juice)
    {
        yield return new WaitForSeconds(juiceLife);

        Destroy(juice);
        afterJuiceEvent?.Invoke();
    }
}
