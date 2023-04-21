using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// 敌人管理器
/// </summary>
public class EnemyManager : Singleton<EnemyManager>
{
    /// <summary>
    /// 随机返回出一个敌人对象，作为追踪子弹的目标，避免使用Transfrom.Find寻找场景对象，消耗性能。
    /// </summary>
    /// <returns></returns>
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    
    /// <summary>
    /// 敌人波数
    /// </summary>
    public int WaveNumber => waveNumber;
    
    /// <summary>
    /// 波数之间的时间间隔
    /// </summary>
    public float TimeBetweenWaves => timeBetweenWaves;

    /// <summary>
    /// 是否生成敌人
    /// </summary>
    [SerializeField] private bool SpawnEnemy = true;

    /// <summary>
    /// 显示波数的UI
    /// </summary>
    [SerializeField] private GameObject waveUI;

    /// <summary>
    /// 敌人预制体列表
    /// </summary>
    [SerializeField] private GameObject[] enemyPrefabs;
    
    /// <summary>
    /// 生成敌人的间隔
    /// </summary>
    [SerializeField] private float timeBetweenSpawns = 1f;

    /// <summary>
    /// 敌人波数的间隔
    /// </summary>
    [SerializeField] private float timeBetweenWaves = 1f;

    /// <summary>
    /// 最少的敌人数量
    /// </summary>
    [SerializeField] private int minEnemyAmount = 4;

    /// <summary>
    /// 最多的敌人数量
    /// </summary>
    [SerializeField] private int maxEnemyAmount = 10;

    
    [Header("======= Boss Settings =======")]
    [SerializeField] private GameObject bossPrefab;

    /// <summary>
    /// Boss的波数
    /// </summary>
    [SerializeField] private int bossWaveNumber;
    
    private int waveNumber = 1;
    private int enemyAmount;

    /// <summary>
    /// 敌人列表
    /// </summary>
    private List<GameObject> enemyList;
    
    private WaitForSeconds waitTimeBetweenSpawns;
    private WaitForSeconds waitTimeBetweenWaves;
    /// <summary>
    /// 等待只到没有敌人
    /// </summary>
    private WaitUntil waitUntillNoEnemy;
    
    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntillNoEnemy = new WaitUntil(()=>enemyList.Count == 0);
    }
    
    IEnumerator Start()
    {
        while (SpawnEnemy && GameManager.GameState != GameState.GameOver)
        {
            waveUI.SetActive(true);
            yield return waitTimeBetweenWaves;
            waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    /// <summary>
    /// 随机生成敌人的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlySpawnCoroutine()
    {
        //若敌人波数 取 Boss波数的余 为0 就生成Boss
        if (waveNumber % bossWaveNumber == 0)
        {
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);
        }
        else
        {
            //敌人数量随着Boss波数的增加而增加
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);
            for (int i = 0; i < enemyAmount; i++)
            {
                //将随机生成的敌人添加到敌人列表中
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
                yield return waitTimeBetweenSpawns;
            }
        }

        yield return waitUntillNoEnemy;
        
        waveNumber++;
    }
    /// <summary>
    /// 从敌人列表中移除敌人
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}