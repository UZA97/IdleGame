using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public int defence;
    public float attackSpeed;
    public float moveSpeed;
     public bool isAttack;

    public virtual void Attack() { }
}
