using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class ShieldBossBehaviour : BasicBossBehaviour
    {
        [SerializeField] private UnityEvent damageEvents;
        [SerializeField] private UnityEvent deathEvents;

        private void Update()
        {
            transform.LookAt(new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z), Vector3.up);
        }

        public override void TakeDamage(int currentHealth, int damageAmount)
        {
            base.TakeDamage(currentHealth, damageAmount);
            damageEvents?.Invoke();
        }

        public override void Death()
        {
            deathEvents?.Invoke();
        }

        public override void RestoreHealth(int currentHealth, int restorationAmount)
        {
            base.RestoreHealth(currentHealth, restorationAmount);
        }
    }
}
