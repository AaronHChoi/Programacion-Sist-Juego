using UnityEngine;

namespace _2.Strategy
{
    public class MagicAttack : MonoBehaviour, IAttackStrategy
    {
        public string Name => "Magic";
        public void Attack()
        {
            Debug.Log("Magic Attack");
        }
    }
}
