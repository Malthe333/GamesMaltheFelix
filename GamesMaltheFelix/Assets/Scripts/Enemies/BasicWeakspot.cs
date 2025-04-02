using UnityEngine;

public class BasicWeakspot : Health
{
    [SerializeField] private Health masterHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (masterHealth == null)
        {
            Health[] healthScripts = GetComponentsInParent<Health>();

            for (int i = 0; i < healthScripts.Length; i++)
            {
                if (healthScripts[i].GetType() != typeof(Health)) continue;
                masterHealth = healthScripts[i];
            }

            if (masterHealth == null)
            {
                Debug.LogWarning($"{name} is a weakspot but has no masterHealth assigned, nor could any be found in its parent");
            }
        }
    }

    public override void Damage(int amount)
    {
        masterHealth.Damage(amount);
    }

    public override void Restore(int amount)
    {
        masterHealth.Restore(amount);
    }
}
