using UnityEngine;

namespace Enemies
{
    public class ShieldBossBehaviour : BasicBossBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(playerTrans.position - transform.position, Vector3.up);
        }

        public override void TakeDamage(int currentHealth, int damageAmount)
        {
            base.TakeDamage(currentHealth, damageAmount);
        }

        public override void Death()
        {
            Debug.Log($"{name} has died, no death animation atm so just being destroyed");
            Destroy(gameObject);
        }

        public override void RestoreHealth(int currentHealth, int restorationAmount)
        {
            base.RestoreHealth(currentHealth, restorationAmount);
        }
    }
}
