using UnityEngine;

public class NonePowerUp : IPowerUpStrategy
{
    public string Name => "NonePowerUp";
    public void PowerUp(Simulacro.PlayerMovement player)
    {
        Debug.Log("No Power Up!");
    }

}
