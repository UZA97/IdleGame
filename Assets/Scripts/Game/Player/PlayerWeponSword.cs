using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeponSword : Weapon
{
    public override void Attack()
    {
        base.Attack();

    }
    public void SetWeaponInfo(Weapon info)
    {

    }
    private void OnTriggerStay(Collider other)
    {
        EnemyInfo enemy;
        if (other.gameObject.tag == ("Enemy"))
        {
            Debug.Log("Collider : " + other.name);
            enemy = other.gameObject.GetComponent<EnemyInfo>();
            enemy.Hit(damage);
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("ControllerColliderHit : " + hit.gameObject.name);
        EnemyInfo enemy;
        if (hit.gameObject.tag == ("Enemy"))
        {
            enemy = hit.gameObject.GetComponent<EnemyInfo>();
            enemy.Hit(damage);
        }
    }

}
