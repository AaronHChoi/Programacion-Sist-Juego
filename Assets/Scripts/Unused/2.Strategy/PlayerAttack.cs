using System;
using UnityEngine;

namespace _2.Strategy
{
    public class PlayerAttack : MonoBehaviour
    {
        private IAttackStrategy _current = new UnarmedAttack();
        [SerializeField] private float speed = 5f;


        public void SetStrategy(IAttackStrategy strategy)
        {
            _current = strategy ?? new UnarmedAttack();
            Debug.Log($"Equipped Weapon: {_current.Name}");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _current.Attack();
            }
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
        
            transform.Translate(horizontal * Time.deltaTime * speed, 0, vertical * Time.deltaTime * speed);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            IAttackStrategy weapon = other.GetComponent<IAttackStrategy>();
            if (weapon == null) return;
            
            SetStrategy(weapon);
            
            Debug.Log($"[Pickup] Equipped: {weapon.Name}");
        }
    }
}
