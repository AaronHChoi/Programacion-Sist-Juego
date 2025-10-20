using UnityEngine;
using System;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    public event Action<int> OnAmmoChanged;

    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;
    private bool infiniteAmmo = false; // Flag para munición infinita
    private Coroutine infiniteAmmoCoroutine; // Referencia a la coroutine activa

    void Awake()
    {
        Instance = this;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public bool TryUseAmmo()
    {
        // Si tiene munición infinita, siempre retorna true
        if (infiniteAmmo)
        {
            OnAmmoChanged?.Invoke(-1); // -1 indica infinito en UI
            return true;
        }

        if (currentAmmo <= 0) return false;

        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo);
        return true;
    }

    public void Reload()
    {
        if (infiniteAmmo) return; // No recargar si tiene munición infinita

        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    public void SetInfiniteAmmo(bool active)
    {
        infiniteAmmo = active;

        if (active)
        {
            OnAmmoChanged?.Invoke(-1); // Notificar UI
        }
        else
        {
            currentAmmo = maxAmmo; // Restaurar munición al terminar
            OnAmmoChanged?.Invoke(currentAmmo);
        }
    }

    // Nuevo método para activar munición infinita por duración
    public void ActivateInfiniteAmmoForDuration(float duration)
    {
        // Si ya hay una coroutine activa, detenerla
        if (infiniteAmmoCoroutine != null)
        {
            StopCoroutine(infiniteAmmoCoroutine);
        }

        infiniteAmmoCoroutine = StartCoroutine(InfiniteAmmoCoroutine(duration));
    }

    private IEnumerator InfiniteAmmoCoroutine(float duration)
    {
        SetInfiniteAmmo(true);
        Debug.Log($"Infinite Ammo active for {duration} seconds");

        yield return new WaitForSeconds(duration);

        SetInfiniteAmmo(false);
        Debug.Log("Infinite Ammo expired - Back to normal");

        infiniteAmmoCoroutine = null;
    }
}