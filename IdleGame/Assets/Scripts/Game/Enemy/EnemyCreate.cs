using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    [Range(1, 300)]
    [SerializeField] int enemyPoolSize;
    [SerializeField] Transform enemyPoolPos;
    [SerializeField] Transform[] createPos;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Enemy[] enemyInfos;

    private GameObject player;

    private int wave = 0;
    public Queue<EnemyInfo> enemyPool = new Queue<EnemyInfo>();
    private Queue<GameObject> enemyCreate = new Queue<GameObject>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        EnemyCreatePool();
    }

    private void EnemyCreatePool()
    {
        GameObject go;
        for (int i = 0; i < enemyPoolSize; i++)
        {
            go = Instantiate(enemyPrefabs[GetEnemyCreateType()], enemyPoolPos);
            EnemyInfo info = go.GetComponent<EnemyInfo>();
            info.Enemy = enemyInfos[wave];
            info.Player = player;

            enemyPool.Enqueue(info);
            go.SetActive(false);
        }
    }
    private int GetEnemyCreateType()
    {
        return Random.Range(0, enemyPrefabs.Length);
    }
    private Transform GetEnemyCreatePosition()
    {
        int idx = Random.Range(0, createPos.Length);

        return createPos[idx];
    }

    //public void StartEnemyWave(int _wave, int _count)
    public void StartEnemyWave(int _wave)
    {
        StartCoroutine(StartEnemyCreateCoroutine(_wave, 1));
    }
    private IEnumerator StartEnemyCreateCoroutine(int _wave, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            EnemyInfo info = enemyPool.Dequeue();
            enemyCreate.Enqueue(info.gameObject);
            info.Enemy = enemyInfos[_wave];
            info.transform.SetParent(GetEnemyCreatePosition());
            info.transform.localPosition = Vector3.zero;
            info.gameObject.SetActive(true);
            info.NPC_Start();

            yield return new WaitForSeconds(1f);
        }

        yield break;
    }



}
