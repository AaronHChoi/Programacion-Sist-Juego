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
        [SerializeField] private float fireRate = 5f; // bullets per second
        private float lastFireTime = -999f;
        [SerializeField] private LayerMask aimLayerMask = ~0; // layers to aim against (default: everything)
        
        [Header("Health")]
        public int health = 100;
        
        [Header("Visuals")]
        [SerializeField] private Transform weaponTransform; // Referencia al arma
        [SerializeField] private Transform carBodyTransform; // Referencia al modelo del auto (puedes asignar manualmente)
        [Header("PowerUp Visual Meshes")] 
        [SerializeField] private GameObject redMesh;   // activado cuando invulnerable
        [SerializeField] private GameObject blueMesh;  // activado cuando ammo infinita
        [SerializeField] private GameObject purpleMesh;// activado cuando ambos
        
        private bool isInvulnerable = false;
        private bool hasInfiniteAmmo = false;
        private Coroutine invulnerabilityCoroutine;

        private PlayerState _currentState;

        public event Action OnDied;
        public event Action<string> OnStateChanged; // Nuevo evento para UI

        void Awake()
        {
            // If carBodyTransform not assigned in Inspector, try to find a sensible default
            if (carBodyTransform == null)
            {
                // Prefer child named like "truck" or similar
                Transform found = transform.Find("truck.001");
                if (found == null)
                {
                    // fallback: first child that has a Renderer
                    foreach (Transform child in transform)
                    {
                        if (child.GetComponentInChildren<Renderer>() != null)
                        {
                            found = child;
                            break;
                        }
                    }
                }

                if (found != null)
                {
                    carBodyTransform = found;
                }
            }
        }

        void OnEnable()
        {
            if (AmmoManager.Instance != null)
            {
                AmmoManager.Instance.OnInfiniteAmmoStateChanged += HandleInfiniteAmmoChanged;
            }
        }

        void OnDisable()
        {
            if (AmmoManager.Instance != null)
            {
                AmmoManager.Instance.OnInfiniteAmmoStateChanged -= HandleInfiniteAmmoChanged;
            }
        }

        void Start()
        {
            _currentState = new IdleState();
            OnStateChanged?.Invoke("Idle"); // Notifica estado inicial

            UpdatePowerUpMeshes();
        }

        // Public helper to allow assigning the car body at runtime (from inspector or other scripts)
        public void SetCarBodyTransform(Transform t)
        {
            carBodyTransform = t;
        }

        void Update()
        {
            _currentState.Handle(this);
            
            // Aim firePoint & weapon towards mouse position
            AimTowardsMouse();

            // Hold fire button to shoot respecting fire rate
            if (Input.GetButton("Fire1"))
            {
                FireIfReady();
             
            }
        }

        private void AimTowardsMouse()
        {
            if (firePoint == null) return;
            Camera cam = Camera.main;
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            // Option A: Raycast against colliders
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimLayerMask))
            {
                Vector3 target = hit.point;
                Vector3 dir = (target - firePoint.position);
                dir.y = 0f; // keep flat on ground plane if needed
                if (dir.sqrMagnitude > 0.0001f)
                {
                    Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    firePoint.rotation = rot;
                    if (weaponTransform != null) weaponTransform.rotation = rot;
                }
                return;
            }

            // Option B: fallback to plane at player height
            Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 target = ray.GetPoint(enter);
                Vector3 dir = (target - firePoint.position);
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    firePoint.rotation = rot;
                    if (weaponTransform != null) weaponTransform.rotation = rot;
                }
            }
        }

        public void FireIfReady()
        {
            float interval = fireRate > 0 ? (1f / fireRate) : 0f;
            if (Time.time - lastFireTime < interval) return;

            // Try to use ammo or auto-reload
            bool fired = false;
            if (AmmoManager.Instance != null)
            {
                if (AmmoManager.Instance.IsReloading)
                {
                    // skip firing during reload
                }
                else if (AmmoManager.Instance.TryUseAmmo())
                {
                    fired = SpawnBullet();
                }
                else
                {
                    // start incremental reload when empty
                    AmmoManager.Instance.StartReload();
                }
            }
            else
            {
                // No ammo manager, still spawn bullet
                fired = SpawnBullet();
            }

            if (fired)
            {
                lastFireTime = Time.time;
                OnStateChanged?.Invoke("Shooting");
                SoundManager.Instance?.PlayPlayerShoot();
            }
        }

        private bool SpawnBullet()
        {
            if (BulletFactory.Instance == null || bulletData == null || firePoint == null) return false;
            var bullet = BulletFactory.Instance.CreateBullet(
                bulletData,
                firePoint.position,
                firePoint.rotation
            );
            return bullet != null;
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
            SoundManager.Instance?.PlayPlayerDamaged();

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
            UpdatePowerUpMeshes();
        }

        private void HandleInfiniteAmmoChanged(bool active)
        {
            hasInfiniteAmmo = active;
            UpdatePowerUpMeshes();
        }

        private void UpdatePowerUpMeshes()
        {
            if (redMesh != null) redMesh.SetActive(false);
            if (blueMesh != null) blueMesh.SetActive(false);
            if (purpleMesh != null) purpleMesh.SetActive(false);

            if (isInvulnerable && hasInfiniteAmmo)
            {
                if (purpleMesh != null) purpleMesh.SetActive(true);
            }
            else if (isInvulnerable)
            {
                if (redMesh != null) redMesh.SetActive(true);
            }
            else if (hasInfiniteAmmo)
            {
                if (blueMesh != null) blueMesh.SetActive(true);
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