using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy
{
    public int hp;      // 체력
    public int atk;     // 공격력
    public int def;     // 방어력
    public float speed; // 스피드
}

public class EnemyInfo : MonoBehaviour
{
    const float DIE_DELAY = 3f;
    [SerializeField] Enemy enemy;
    public Enemy Enemy { set { enemy = value; } }

    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] NpcState npcState;
    private NpcAniState npcAniState;


    [HideInInspector] public GameObject Player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void NPC_Start()
    {
        agent.speed = enemy.speed;
        npcState = NpcState.alive;
        AnimationChanger(NpcAniState.Walk);
        agent.SetDestination(Player.transform.position);
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (npcState.Equals(NpcState.alive))
        {
            yield return null;
            if (MoveCheck())
            {
                Debug.Log(1);
                if (agent.isStopped)
                {
                    AnimationChanger(NpcAniState.Walk);
                    agent.SetDestination(Player.transform.position);
                }
            }
            else
            {
                Debug.Log(2);
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }
                yield return new WaitForSeconds(1f);
                AnimationChanger(NpcAniState.Attack01);
            }
        }

        AnimationChanger(NpcAniState.DieStart);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(DIE_DELAY);
        AnimationChanger(NpcAniState.DieEnd);
        this.gameObject.SetActive(false);
    }
    private void AnimationChanger(NpcAniState state)
    {
        npcAniState = state;
        animator.Play(state.ToString(), 0, 0);
    }
    private bool MoveCheck()
    {
        // Vector3 targetPos = new Vector3(Player.transform.position.x,0,Player.transform.position.z);
        if (agent.remainingDistance < agent.stoppingDistance)
            return false;
        else
            return true;

    }

}
