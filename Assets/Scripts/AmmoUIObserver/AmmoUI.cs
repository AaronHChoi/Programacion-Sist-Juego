using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    void OnEnable()
    {
        AmmoManager.Instance.OnAmmoChanged += UpdateUI;
    }

    void OnDisable()
    {
        AmmoManager.Instance.OnAmmoChanged -= UpdateUI;
    }

    void UpdateUI(int ammo)
    {
        ammoText.text = $"Ammo: {ammo}";    
    }
}