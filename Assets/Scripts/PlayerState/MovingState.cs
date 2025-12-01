using Simulacro;
using UnityEngine;

public class MovingState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // Movimiento hacia adelante/atrás
        Vector3 moveDirection = player.transform.forward * v;
        player.transform.Translate(moveDirection * Time.deltaTime * player.speed, Space.World);

        // Rotación del auto (solo en el eje Y)
        if (Mathf.Abs(h) > 0.01f)
        {
            float rotation = h * player.rotationSpeed * Time.deltaTime;
            player.transform.Rotate(0, rotation, 0);
        }

        if (h == 0 && v == 0) 
            player.SetState(new IdleState());

        if (Input.GetKeyDown(KeyCode.Space))
            player.SetState(new ShootingState());
    }
}