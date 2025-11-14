using UnityEngine;

public class NonePowerUp : IPowerUpStrategy
{
    public string Name => "NonePowerUp";
    public void PowerUp()
    {
        Debug.Log("No Power Up!");
    }

}
