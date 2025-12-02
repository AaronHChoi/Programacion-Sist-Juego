using Simulacro;
using UnityEngine;

public class IdleState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            player.SetState(new MovingState());

        // Replace spacebar with Fire1 logic: hold/click to start shooting state
        if (Input.GetButton("Fire1"))
            player.SetState(new ShootingState());
    }
}