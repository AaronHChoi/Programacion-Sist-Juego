using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackStrategy
{
    public string _name => "Melee";
    public void Attack()
    {
        Debug.Log("Melee Attack");
    }
}
