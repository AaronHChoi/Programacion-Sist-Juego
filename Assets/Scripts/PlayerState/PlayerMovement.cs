using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
        private Coroutine colorCoroutine;

        private PlayerState _currentState;

        // Notify observers when the player dies
        public event Action OnDied;

        // Guardamos los colores originales de todos los renderers/materiales
        private struct RendererOriginalColors
        {
            public Renderer renderer;
            public Color[] colors;
        }
        private List<RendererOriginalColors> _originals = new List<RendererOriginalColors>();

        void Start()
        {
            _currentState = new IdleState();

            // Recolectar todos los renderers (incluye hijos) y guardar sus colores originales
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                var mats = rend.materials; // esto puede instanciar materiales, pero nos permite modificar colores por instancia
                var colors = new Color[mats.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    // Verificamos que el material tenga la propiedad de color
                    if (mats[i].HasProperty("_Color"))
                        colors[i] = mats[i].color;
                    else
                        colors[i] = Color.white;
                }
                _originals.Add(new RendererOriginalColors { renderer = rend, colors = colors });
            }
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
            // Notify listeners first so they can handle respawn logic
            OnDied?.Invoke();
            // Deactivate after notifying; external systems can reactivate on next frame
            gameObject.SetActive(false);
        }

        // Cambiado: la invulnerabilidad pinta de rojo
        public void SetInvulnerable(bool invulnerable)
        {
            isInvulnerable = invulnerable;

            if (invulnerable)
            {
                // Si hay una corutina de color temporal, detenerla (invulnerabilidad tiene prioridad)
                if (colorCoroutine != null)
                {
                    StopCoroutine(colorCoroutine);
                    colorCoroutine = null;
                }

                SetAllRenderersColor(Color.red);
            }
            else
            {
                RestoreOriginalColors();
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

        // Nuevo: aplicar un color temporal (ej. azul para balas infinitas)
        // No aplicará si actualmente hay invulnerabilidad activa (prioridad roja).
        public void ApplyPowerUpColorForDuration(Color color, float duration)
        {
            if (isInvulnerable) return;

            if (colorCoroutine != null)
            {
                StopCoroutine(colorCoroutine);
            }

            colorCoroutine = StartCoroutine(PowerUpColorCoroutine(color, duration));
        }

        private IEnumerator PowerUpColorCoroutine(Color color, float duration)
        {
            SetAllRenderersColor(color);
            yield return new WaitForSeconds(duration);
            RestoreOriginalColors();
            colorCoroutine = null;
        }

        private void SetAllRenderersColor(Color color)
        {
            foreach (var entry in _originals)
            {
                var rend = entry.renderer;
                var mats = rend.materials; // obtener instancias para poder asignar
                for (int i = 0; i < mats.Length; i++)
                {
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = color;
                }
                rend.materials = mats;
            }
        }

        private void RestoreOriginalColors()
        {
            foreach (var entry in _originals)
            {
                var rend = entry.renderer;
                var mats = rend.materials;
                int len = Mathf.Min(mats.Length, entry.colors.Length);
                for (int i = 0; i < len; i++)
                {
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = entry.colors[i];
                }
                rend.materials = mats;
            }
        }
    }
}