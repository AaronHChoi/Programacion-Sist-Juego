using UnityEngine;

namespace _2.Factory
{
    public class Zombie : Enemy
    {
        public override void Attack()
        {
            Debug.Log("ZombieAttack");
        }
    }
}
