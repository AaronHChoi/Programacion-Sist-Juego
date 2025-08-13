using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour, IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("Magic Attack");
    }
}
