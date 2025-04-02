using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [HideInInspector, Tooltip("Current health")] private int health;

    [HideInInspector] public UnityEvent deathEvents;

    [HideInInspector] public UnityEvent<int, int> damageEvents;

    [HideInInspector] public UnityEvent<int, int> healingEvents;

    void Awake()
    {
        health = maxHealth;
    }

    public int CheckMaxHealth()
    {
        return maxHealth;
    }

    public int CheckCurrentHealth()
    {
        return health;
    }

    /// <summary>
    /// Whenever you wish to damage something, call this function
    /// </summary>
    /// <param name="amount"></param>
    public virtual void Damage(int amount)
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

    public virtual void Restore(int amount)
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
