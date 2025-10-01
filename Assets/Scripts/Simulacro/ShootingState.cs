using Simulacro;
using UnityEngine;

public class ShootingState : PlayerState
{
    public override void Handle(PlayerMovement player)
    {
        if (AmmoManager.Instance.TryUseAmmo())
        {
            BulletFactory.Instance.CreateBullet(
                player.bulletData,
                player.firePoint.position,
                player.firePoint.rotation
            );
        }

        player.SetState(new IdleState());
    }
}