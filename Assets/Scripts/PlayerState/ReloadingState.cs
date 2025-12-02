using Simulacro;
using UnityEngine;

public class ReloadingState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        // Block firing; PlayerMovement already checks AmmoManager.IsReloading
        // Keep aiming via PlayerMovement.Update

        // If finished reloading, decide next state
        if (AmmoManager.Instance != null && !AmmoManager.Instance.IsReloading)
        {
            if (Input.GetButton("Fire1"))
            {
                player.SetState(new ShootingState());
            }
            else
            {
                player.SetState(new IdleState());
            }
        }
    }
}
