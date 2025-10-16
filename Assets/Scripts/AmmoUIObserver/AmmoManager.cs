using UnityEngine;
using System;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;
    public event Action<int> OnAmmoChanged;

    [SerializeField] private int maxAmmo = 10;
    private int currentAmmo;

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
        if (currentAmmo <= 0) return false;

        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo);
        return true;
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo);
    }
}