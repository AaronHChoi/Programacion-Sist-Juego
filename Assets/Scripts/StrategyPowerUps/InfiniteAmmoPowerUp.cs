using UnityEngine;

public class InfiniteAmmoPowerUp : MonoBehaviour, IPowerUpStrategy
{
    public string Name => "InfiniteAmmoPowerUp";

    [SerializeField] private float duration = 10f; // Duración del power-up

    public void PowerUp()
    {
        Debug.Log("Infinite Ammo Power Up Activated!");

        if (AmmoManager.Instance != null)
        {
            // Ejecutar la coroutine en el AmmoManager en lugar de en este objeto
            AmmoManager.Instance.ActivateInfiniteAmmoForDuration(duration);
        }
    }
}