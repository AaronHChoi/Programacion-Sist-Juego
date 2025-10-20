using UnityEngine;

namespace _2.Strategy
{
    public class MeleeAttack : MonoBehaviour, IAttackStrategy
    {
        public string Name => "Melee";
        public void Attack()
        {
            Debug.Log("Melee Attack");
        }
    }
}
