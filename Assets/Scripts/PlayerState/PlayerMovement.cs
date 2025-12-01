using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Simulacro
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 5f;
        public float rotationSpeed = 200f; // Nueva velocidad de rotación
        
        [Header("Shooting")]
        public BulletData bulletData;
        public Transform firePoint;
        
        [Header("Health")]
        public int health = 100;
        
        [Header("Visuals")]
        [SerializeField] private Transform weaponTransform; // Referencia al arma
        [SerializeField] private Transform carBodyTransform; // Referencia al modelo del auto
        
        private bool isInvulnerable = false;
        private Coroutine invulnerabilityCoroutine;
        private Coroutine colorCoroutine;

        private PlayerState _currentState;

        public event Action OnDied;
        public event Action<string> OnStateChanged; // Nuevo evento para UI

        private struct RendererOriginalColors
        {
            public Renderer renderer;
            public Color[] colors;
        }
        private List<RendererOriginalColors> _originals = new List<RendererOriginalColors>();

        void Start()
        {
            _currentState = new IdleState();
            OnStateChanged?.Invoke("Idle"); // Notifica estado inicial

            // Guardar colores originales del auto (no del arma)
            if (carBodyTransform != null)
            {
                var renderers = carBodyTransform.GetComponentsInChildren<Renderer>();
                foreach (var rend in renderers)
                {
                    var mats = rend.materials;
                    var colors = new Color[mats.Length];
                    for (int i = 0; i < mats.Length; i++)
                    {
                        if (mats[i].HasProperty("_Color"))
                            colors[i] = mats[i].color;
                        else
                            colors[i] = Color.white;
                    }
                    _originals.Add(new RendererOriginalColors { renderer = rend, colors = colors });
                }
            }

            // Asegurar que el arma apunte siempre hacia adelante
            if (weaponTransform != null)
            {
                weaponTransform.localRotation = Quaternion.identity;
            }
        }

        void Update()
        {
            _currentState.Handle(this);
            
            // Mantener el arma apuntando hacia adelante (en caso de que se desvíe)
            if (weaponTransform != null)
            {
                weaponTransform.rotation = Quaternion.Euler(0, 0, 0); // Siempre hacia adelante
            }
        }

        public void SetState(PlayerState newState)
        {
            _currentState = newState;
            
            // Notificar cambio de estado para la UI
            string stateName = newState.GetType().Name.Replace("State", "");
            OnStateChanged?.Invoke(stateName);
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
            OnDied?.Invoke();
            gameObject.SetActive(false);
        }

        public void SetInvulnerable(bool invulnerable)
        {
            isInvulnerable = invulnerable;

            if (invulnerable)
            {
                if (colorCoroutine != null)
                {
                    StopCoroutine(colorCoroutine);
                    colorCoroutine = null;
                }

                SetCarColor(Color.red); // Solo cambia color del auto
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

        //Aplicar color solo al auto (no al arma)
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
            SetCarColor(color);
            yield return new WaitForSeconds(duration);
            RestoreOriginalColors();
            colorCoroutine = null;
        }

        private void SetCarColor(Color color)
        {
            foreach (var entry in _originals)
            {
                var rend = entry.renderer;
                var mats = rend.materials;
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