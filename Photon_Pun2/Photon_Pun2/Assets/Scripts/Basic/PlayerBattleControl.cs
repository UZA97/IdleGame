using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer hpBar;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackDistance;

    private PlayerAniControl playerAniControl;
    public bool isDie => hpBar.size.x <= 0;

    private void Awake()
    {
        playerAniControl = this.GetComponent<PlayerAniControl>();
    }

    private void Start()
    {
        InitHP();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Attack();
        }
    }
    private void InitHP()
    {
        hpBar.size = Vector2.one;
    }

    public void Hit(float demage)
    {
        float hp = hpBar.size.x;
        float result = hp - demage;

        if (hp > 0)
        {
            if (result > 0)
            {
                hpBar.size = new Vector2(result, 1);
            }
            else
            {
                hpBar.size = new Vector2(0, 1);
            }
            playerAniControl.AnimationChange(PlayerAnimationType.Damage);
        }
    }

    public void Attack()
    {
        RaycastHit[] hits = Physics.RaycastAll(attackPos.position, this.transform.forward, attackDistance, 1 << 6);

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].collider.GetComponent<PlayerBattleControl>().Hit(0.1f);
        }
        playerAniControl.AnimationChange(PlayerAnimationType.Attack);
    }

    // private void OnGUI()
    // {
    //     Gizmos.DrawLine(attackPos.position, new Vector3(attackPos.position.x, attackPos.position.y, attackPos.position.z + attackDistance));
    // }
    // private void OnDrawGizmos()
    // {

    // }
}