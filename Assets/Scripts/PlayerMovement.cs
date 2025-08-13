using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private IAttackStrategy _currentWeapon;
    public float speed = 5f;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            AttackAction();
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));
        }
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
    
}
