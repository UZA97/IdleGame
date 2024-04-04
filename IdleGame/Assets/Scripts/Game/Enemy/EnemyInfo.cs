using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy
{
    public float hp;      // 체력
    public float atk;     // 공격력
    public float atkSpeed; // 공격속도
    public float def;     // 방어력
    public float speed; // 스피드
}

public class EnemyInfo : MonoBehaviour
{
    const float NOMALIZE = 0.01f;

    const float DIE_DELAY = 3f;
    [SerializeField] Enemy enemy;
    public Enemy Enemy { set { enemy = value; } }

    [SerializeField] GameObject hp;

    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] NpcState npcState;
    private NpcAniState npcAniState;

    public GameObject Player { private get; set; }

    private bool isAttack = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void NPC_Start()
    {
        isAttack = false;
        agent.speed = enemy.speed;

        SetNPCState(NpcState.alive);
        Move(Player.transform.position);
    }

    private void Update()
    {
        switch (npcState)
        {
            case NpcState.alive:
                if (MoveCheck())
                {
                    isAttack = false;
                    Move(Player.transform.position);
                }
                else
                {
                    if (!agent.isStopped)
                    {
                        agent.isStopped = true;
                        agent.ResetPath();
                    }
                    Attack();
                }
                break;
            case NpcState.stop:
                break;
            case NpcState.die:
                break;
        }
    }

    private void SetNPCState(NpcState state)
    {
        npcState = state;
    }

    private void AnimationChanger(NpcAniState state)
    {
        npcAniState = state;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(state.ToString()))
        {
            animator.Play(state.ToString(), 0, 0);
        }
    }

    #region Move
    private bool MoveCheck()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            return false;
        else
            return true;
    }
    private void Move(Vector3 position)
    {
        agent.SetDestination(position);
        AnimationChanger(NpcAniState.Walk);
    }
    #endregion

    #region Attack
    private void Attack()
    {
        isAttack = true;
        StartCoroutine(AttackCoroutine());
    }
    private IEnumerator AttackCoroutine()
    {
        while (isAttack)
        {
            AnimationChanger(NpcAniState.Attack01);
            yield return new WaitForSeconds(enemy.atkSpeed);
        }
    }
    #endregion

    #region Hit
    public void Hit(float damege)
    {
        float hpClamp = hp.transform.localScale.x - (damege * enemy.def * NOMALIZE);

        hp.transform.localScale = new Vector3(Mathf.Clamp(hpClamp, 0, 1), 1);
    }
    #endregion

    #region Die
    private void Die()
    {
        StartCoroutine(DieCoroutine());
    }
    private IEnumerator DieCoroutine()
    {
        AnimationChanger(NpcAniState.DieStart);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(DIE_DELAY);
        AnimationChanger(NpcAniState.DieEnd);

        // 다시 풀에 넣기
        this.gameObject.SetActive(false);
    }
    #endregion

    #region Debug
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     if (agent.remainingDistance > agent.stoppingDistance)
    //     {
    //         for (int i = 0; i < agent.path.corners.Length - 1; i++)
    //         {
    //             Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
    //             Gizmos.DrawSphere(agent.path.corners[i + 1], .5f);
    //         }

    //         Gizmos.DrawSphere(agent.pathEndPosition, 1f);
    //     }
    // }
    #endregion
}
