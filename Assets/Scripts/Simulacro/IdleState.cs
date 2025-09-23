using UnityEngine;

public class IdleState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            player.SetState(new MovingState());

        if (Input.GetKeyDown(KeyCode.Space))
            player.SetState(new ShootingState());
    }
}