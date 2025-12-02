using UnityEngine;
using System;
using System.Collections;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }
    public event Action<int> OnAmmoChanged;
    public event Action<bool> OnInfiniteAmmoStateChanged; // notify visuals
    public event Action<bool> OnReloadStateChanged; // true when reloading

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float reloadDelayPerBullet = 0.25f; // time to add each bullet
    [SerializeField] private float reloadStartDelay = 0.0f; // optional delay before starting reload

    private int currentAmmo;
    private bool infiniteAmmo = false; 
    private Coroutine infiniteAmmoCoroutine; 
    private Coroutine reloadCoroutine;
    private bool isReloading = false;
    private SoundManager soundManager;

    public bool IsReloading => isReloading;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentAmmo = maxAmmo;
        // cache SoundManager if available
        soundManager = SoundManager.Instance ?? FindObjectOfType<SoundManager>();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
    }

    public bool TryUseAmmo()
    {
        if (infiniteAmmo)
        {
            OnAmmoChanged?.Invoke(-1); 
            return true;
        }

        if (isReloading) return false; // cannot shoot while reloading
        if (currentAmmo <= 0) return false;

        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo);
        return true;
    }

    public void StartReload()
    {
        if (infiniteAmmo) return; 
        if (isReloading) return;
        if (currentAmmo >= maxAmmo) return;

        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
        // play reload start sound
        soundManager?.PlayReload();
        reloadCoroutine = StartCoroutine(ReloadCoroutine());
    }

    // Backward compatibility
    public void Reload()
    {
        StartReload();
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        OnReloadStateChanged?.Invoke(true);

        if (reloadStartDelay > 0f)
        {
            yield return new WaitForSeconds(reloadStartDelay);
        }

        // Fill one by one
        while (currentAmmo < maxAmmo && !infiniteAmmo)
        {
            yield return new WaitForSeconds(reloadDelayPerBullet);
            currentAmmo = Mathf.Min(maxAmmo, currentAmmo + 1);
            OnAmmoChanged?.Invoke(currentAmmo);
        }

        isReloading = false;
        OnReloadStateChanged?.Invoke(false);
        reloadCoroutine = null;
        // play reload complete sound
        soundManager?.PlayReloadComplete();
    }

    public void SetInfiniteAmmo(bool active)
    {
        infiniteAmmo = active;

        // notify UI and visuals
        OnInfiniteAmmoStateChanged?.Invoke(active);

        if (active)
        {
            // cancel any reload
            if (reloadCoroutine != null)
            {
                StopCoroutine(reloadCoroutine);
                reloadCoroutine = null;
            }
            isReloading = false;
            OnReloadStateChanged?.Invoke(false);

            // signal reload cancelled/completed
            soundManager?.PlayReloadComplete();

            OnAmmoChanged?.Invoke(-1); 
        }
        else
        {
            // when infinite ends, keep currentAmmo as is
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