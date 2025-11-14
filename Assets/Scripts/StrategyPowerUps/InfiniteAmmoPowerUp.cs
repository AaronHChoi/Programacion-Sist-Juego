using UnityEngine;

public class InfiniteAmmoPowerUp : MonoBehaviour, IPowerUpStrategy
{
    public string Name => "InfiniteAmmoPowerUp";
    [SerializeField] private float duration = 8f;

    public void PowerUp(Simulacro.PlayerMovement player)
    {
        if (player == null) return;

        // Aplica color azul durante la duración del powerup
        player.ApplyPowerUpColorForDuration(Color.blue, duration);

        // Si quieres la lógica real de balas infinitas, implementa un método en PlayerMovement
        // por ejemplo: player.EnableInfiniteAmmo(duration);
    }
}