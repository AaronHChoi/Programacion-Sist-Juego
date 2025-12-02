using UnityEngine;

namespace Simulacro.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public int health = 50;

        private SoundManager soundManager;

        void Start()
        {
            // Prefer singleton, fallback to Find
            soundManager = SoundManager.Instance ?? FindObjectOfType<SoundManager>();
            if (soundManager == null)
            {
                Debug.LogWarning("SoundManager not found in scene. Enemy audio calls will be ignored.");
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            // Play a hit SFX if provided in SoundManager, otherwise ignore
            soundManager?.PlaySfx("Other_0"); // add dedicated method/clip if you want
            if (health <= 0) Die();
        }

        private void Die()
        {
            // Play enemy death via facade
            soundManager?.PlayEnemyDeath();

            // Handle enemy death (destroy, play animation, drop loot, etc.)
            Destroy(gameObject);
        }
    }
}
