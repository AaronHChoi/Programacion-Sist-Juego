using Simulacro;
using UnityEngine;

public class MovingState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        player.transform.Translate(new Vector3(h, 0, v) * Time.deltaTime * player.speed);

        if (h == 0 && v == 0) player.SetState(new IdleState());

        if (Input.GetKeyDown(KeyCode.Space))
            player.SetState(new ShootingState());
    }
}