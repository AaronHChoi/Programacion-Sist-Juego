using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("Ranged Attack");
    }
}
