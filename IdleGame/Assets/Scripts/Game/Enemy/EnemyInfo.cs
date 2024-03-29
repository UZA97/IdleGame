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
    [SerializeField] Enemy enemy;
    public Enemy Enemy { set { enemy = value; } }

    [HideInInspector] public GameObject Player;

    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartMove()
    {
        agent.speed = enemy.speed;
        agent.SetDestination(Player.transform.position);
    }
}
