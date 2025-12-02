    using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AmmoUI : MonoBehaviour
{
    [Header("Ammo Display")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Transform bulletStackContainer; // Contenedor para balas visuales
    [SerializeField] private GameObject bulletIconPrefab; // Prefab de ícono de bala
    [SerializeField] private float bulletIconSpacing = 30f;

    [Header("State Display")]
    [SerializeField] private TextMeshProUGUI stateText; // Texto para mostrar estado
    [SerializeField] private Image stateIndicator; // Imagen de fondo del estado
    [SerializeField] private Color idleColor = Color.gray;
    [SerializeField] private Color movingColor = Color.green;
    [SerializeField] private Color shootingColor = Color.red;

    private List<GameObject> bulletIcons = new List<GameObject>();

    void OnEnable()
    {
        if (AmmoManager.Instance != null)
            AmmoManager.Instance.OnAmmoChanged += UpdateUI;

        // Suscribirse al evento de cambio de estado del jugador
        var player = FindFirstObjectByType<Simulacro.PlayerMovement>();
        if (player != null)
        {
            player.OnStateChanged += UpdateStateDisplay;
        }
    }

    void OnDisable()
    {
        if (AmmoManager.Instance != null)
            AmmoManager.Instance.OnAmmoChanged -= UpdateUI;

        var player = FindFirstObjectByType<Simulacro.PlayerMovement>();
        if (player != null)
        {
            player.OnStateChanged -= UpdateStateDisplay;
        }
    }

    private void UpdateUI(int currentAmmo)
    {
        if (currentAmmo == -1)
        {
            ammoText.text = "AMMO: ?";
            ClearBulletStack();
        }
        else
        {
            ammoText.text = $"AMMO: {currentAmmo}";
            UpdateBulletStack(currentAmmo);
        }
    }

    private void UpdateBulletStack(int ammoCount)
    {
        // Limpiar íconos existentes
        ClearBulletStack();

        // Crear nuevos íconos
        for (int i = 0; i < ammoCount; i++)
        {
            GameObject icon = Instantiate(bulletIconPrefab, bulletStackContainer);
            RectTransform rt = icon.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i * bulletIconSpacing);
            bulletIcons.Add(icon);
        }
    }

    private void ClearBulletStack()
    {
        foreach (var icon in bulletIcons)
        {
            Destroy(icon);
        }
        bulletIcons.Clear();
    }

    private void UpdateStateDisplay(string stateName)
    {
        if (stateText != null)
        {
            stateText.text = $"{stateName.ToUpper()}";
        }

        if (stateIndicator != null)
        {
            stateIndicator.color = stateName switch
            {
                "Idle" => idleColor,
                "Moving" => movingColor,
                "Shooting" => shootingColor,
                _ => Color.white
            };
        }
    }
}