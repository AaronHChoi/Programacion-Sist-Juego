using Simulacro;
using UnityEngine;

public class ShootingState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        // If currently reloading, go to ReloadingState
        if (AmmoManager.Instance != null && AmmoManager.Instance.IsReloading)
        {
            player.SetState(new ReloadingState());
            return;
        }

        // Use PlayerMovement's fire cadence & ammo logic
        player.FireIfReady();

        // If Fire1 released, back to Idle
        if (!Input.GetButton("Fire1"))
        {
            player.SetState(new IdleState());
            return;
        }

        // If reload started due to empty ammo, transition to Reloading
        if (AmmoManager.Instance != null && AmmoManager.Instance.IsReloading)
        {
            player.SetState(new ReloadingState());
        }
    }
}