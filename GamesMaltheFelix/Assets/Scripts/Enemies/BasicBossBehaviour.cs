using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Health))]
    public class BasicBossBehaviour : MonoBehaviour
    {
        protected Transform playerTrans;
        protected Transform echoTrans;
        [SerializeField] private TransformTracker playerFinder;
        [SerializeField] private TransformTracker echoFinder;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Gets player and echo transforms
            if (playerFinder.tracked.Count == 0) Debug.LogError($"{name} could not find player in scene and will not function correctly");
            else playerTrans = playerFinder.tracked[0];
            if (echoFinder.tracked.Count > 0) echoTrans = echoFinder.tracked[0];
        }

        private void OnEnable()
        {
            // Subscribe to death and damage events
            Health health = GetComponent<Health>();
            health.damageEvents.AddListener((currentHealth, damageAmount) => TakeDamage(currentHealth, damageAmount));
            health.deathEvents.AddListener(() => Death());
            health.healingEvents.AddListener((currentHealth, restorationAmount) => RestoreHealth(currentHealth, restorationAmount));
        }

        private void OnDisable()
        {
            // Unsubscribe to death and damage events
            Health health = GetComponent<Health>();
            health.damageEvents.RemoveListener((currentHealth, damageAmount) => TakeDamage(currentHealth, damageAmount));
            health.deathEvents.RemoveListener(() => Death());
            health.healingEvents.RemoveListener((currentHealth, restorationAmount) => RestoreHealth(currentHealth, restorationAmount));
        }

        public virtual void TakeDamage(int currentHealth, int damageAmount)
        {
            Debug.Log($"{name} has no behaviour to being damaged and is currently at {currentHealth} after taking {damageAmount} damage in one hit");
        }

        public virtual void Death()
        {
            Debug.LogWarning($"{name} has no behaviour to being killed, destroying it as a failsafe");
            Destroy(gameObject);
        }

        public virtual void RestoreHealth(int currentHealth, int restorationAmount)
        {
            Debug.Log($"{name} has no behaviour to being damaged and is currently at {currentHealth} after taking {restorationAmount} damage in one hit");
        }
    }
}
