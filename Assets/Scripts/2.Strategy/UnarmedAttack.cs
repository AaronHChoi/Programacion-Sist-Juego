using UnityEngine;

namespace _2.Strategy
{
    public class UnarmedAttack : IAttackStrategy
    {
        public string Name => "Unharmed";
        public void Attack()
        {
            Debug.Log("Unharmed Attack");
        }
    }
}
