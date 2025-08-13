using System;
using System.Collections;
using System.Collections.Generic;
using _2.Factory;
using _2.Strategy;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private IAttackStrategy _currentWeapon;
    [SerializeField] private float speed = 5f;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            AttackAction();
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        transform.Translate(horizontal * Time.deltaTime * speed, 0, vertical * Time.deltaTime * speed);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void AttackAction()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.Attack();
        }
        else
        {
            Debug.Log("No weapon equipped");
        }
    }
    
    public void ChangeWeapon(IAttackStrategy newWeapon)
    {
        _currentWeapon = newWeapon;
        Debug.Log("Weapon changed");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Attack();
        }
    }
}
