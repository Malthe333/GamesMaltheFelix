using UnityEngine;

namespace Enemies
{
    public class ShieldBossBehaviour : BasicBossBehaviour
    {
        private void Update()
        {
            transform.LookAt(new Vector3(playerTrans.position.x, transform.position.y, playerTrans.position.z), Vector3.up);
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
