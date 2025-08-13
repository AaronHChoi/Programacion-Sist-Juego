using UnityEngine;

namespace _2.Factory
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private string id;
        public string Id => id;

        public abstract void Attack();
    }
}
