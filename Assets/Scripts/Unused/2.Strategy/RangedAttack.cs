using UnityEngine;

namespace _2.Strategy
{
    public class RangedAttack : MonoBehaviour, IAttackStrategy
    {
        public string Name => "Ranged";
        public void Attack()
        {
            Debug.Log("Ranged Attack");
        }
    }
}
