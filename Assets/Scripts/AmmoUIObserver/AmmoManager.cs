using UnityEngine;
using System;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    public event Action<int> OnAmmoChanged;

    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;
    private bool infiniteAmmo = false; 
    private Coroutine infiniteAmmoCoroutine; 

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
        if (infiniteAmmo)
        {
            OnAmmoChanged?.Invoke(-1); 
            return true;
        }

        if (currentAmmo <= 0) return false;

        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo);
        return true;
    }

    public void Reload()
    {
        if (infiniteAmmo) return; 

        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    public void SetInfiniteAmmo(bool active)
    {
        infiniteAmmo = active;

        if (active)
        {
            OnAmmoChanged?.Invoke(-1); 
        }
        else
        {
            currentAmmo = maxAmmo; 
            OnAmmoChanged?.Invoke(currentAmmo);
        }
    }

    public void ActivateInfiniteAmmoForDuration(float duration)
    {
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