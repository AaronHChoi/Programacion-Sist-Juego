using UnityEngine;
using System.Collections;

namespace Simulacro
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        public BulletData bulletData;
        public Transform firePoint;
        public int health = 100;
        private bool isInvulnerable = false;
        private Coroutine invulnerabilityCoroutine;

        private PlayerState _currentState;

        void Start()
        {
            _currentState = new IdleState();
        }

        void Update()
        {
            _currentState.Handle(this);
        }

        public void SetState(PlayerState newState)
        {
            _currentState = newState;
        }

        public void TakeDamage(int damage)
        {
            if (isInvulnerable)
            {
                Debug.Log("Player is invulnerable! No damage taken.");
                return;
            }

            health -= damage;
            Debug.Log($"Player hit! Health: {health}");

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died!");
            gameObject.SetActive(false);
        }

        public void SetInvulnerable(bool invulnerable)
        {
            isInvulnerable = invulnerable;

            if (invulnerable)
            {
                var renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.yellow;
                }
            }
            else
            {
                var renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.white;
                }
            }
        }

        public void ActivateInvulnerabilityForDuration(float duration)
        {
            if (invulnerabilityCoroutine != null)
            {
                StopCoroutine(invulnerabilityCoroutine);
            }

            invulnerabilityCoroutine = StartCoroutine(InvulnerabilityCoroutine(duration));
        }

        private IEnumerator InvulnerabilityCoroutine(float duration)
        {
            SetInvulnerable(true);
            Debug.Log($"Invulnerability active for {duration} seconds");

            yield return new WaitForSeconds(duration);

            SetInvulnerable(false);
            Debug.Log("Invulnerability expired - Back to normal");

            invulnerabilityCoroutine = null;
        }
    }
}