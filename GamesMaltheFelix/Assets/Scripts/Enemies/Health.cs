using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth { get; private set; } = 10;
    [HideInInspector, Tooltip("Current health")] public int health { get; private set; }

    [HideInInspector] public UnityEvent deathEvents;

    [HideInInspector] public UnityEvent<int, int> damageEvents;

    [HideInInspector] public UnityEvent<int, int> healingEvents;

    void Awake()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Whenever you wish to damage something, call this function
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount)
    {
#if UNITY_EDITOR
        if (amount < 1) Debug.LogWarning($"{name} is being damaged for {(amount == 0 ? "zero" : "negative amounts:" + amount)}");
#endif

        health -= amount;

        if (health < 0)
        {
            deathEvents?.Invoke();
        }
        else
        {
            damageEvents?.Invoke(health, amount);
        }
    }

    public void Restore(int amount)
    {
#if UNITY_EDITOR
        if (amount < 1) Debug.LogWarning($"{name} is being healed for {(amount == 0 ? "zero" : "negative amounts:" + amount)}");
#endif

        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
            return;
        }

        healingEvents?.Invoke(health, amount);
    }
}
