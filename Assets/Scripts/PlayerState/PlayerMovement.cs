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
        private bool isInvulnerable = false; // Flag de invulnerabilidad
        private Coroutine invulnerabilityCoroutine; // Referencia a la coroutine activa

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
            // Si es invulnerable, no recibe daño
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

            // Opcional: cambiar visual del jugador
            if (invulnerable)
            {
                // Ejemplo: hacer parpadear o cambiar color
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

        // Nuevo m�todo para activar invulnerabilidad por duraci�n
        public void ActivateInvulnerabilityForDuration(float duration)
        {
            // Si ya hay una coroutine activa, detenerla
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