using UnityEngine;

public class InfiniteAmmoPowerUp : MonoBehaviour, IPowerUpStrategy
{
    public string Name => "InfiniteAmmoPowerUp";

    [SerializeField] private float duration = 10f; 

    public void PowerUp()
    {
        Debug.Log("Infinite Ammo Power Up Activated!");

        if (AmmoManager.Instance != null)
        {
            AmmoManager.Instance.ActivateInfiniteAmmoForDuration(duration);
        }
    }
}