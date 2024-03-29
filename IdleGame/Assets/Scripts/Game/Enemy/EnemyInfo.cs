using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Enemy
{
    public int hp;      // ü��
    public int atk;     // ���ݷ�
    public int def;     // ����
    public float speed; // ���ǵ�
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
