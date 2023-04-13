using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    //随机取出一个目标敌人的属性
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    //获取敌人波数数值的属性
    public int WaveNumber => _waveNumber;
    //获取每波间隔时间的属性
    public float TimeBetweenWaves => timeBetweenWaves;
    //是否生成敌人
    [SerializeField] private bool SpawnEnemy = true;
    //存储波数UI对象
    [SerializeField] private GameObject waveUI;
    //敌人预制体数组
    [SerializeField] private GameObject[] enemyPrefabs;
    //敌人生成间隔时间
    [SerializeField] private float timeBetweenSpawns = 1f;
    //等待下一波时间
    [SerializeField] private float timeBetweenWaves = 1f;
    //最小敌人数量
    [SerializeField] private int minEnemyAmount = 4;
    //最大敌人数量
    [SerializeField] private int maxEnemyAmount = 10;
    
    [Header("======= Boss Settings =======")]
    //Boss预制体对象
    [SerializeField] private GameObject bossPrefab;
    //Boss战波数数值变量
    [SerializeField] private int bossWaveNumber;
    
    //敌人波数变量，默认为1
    private int _waveNumber = 1;
    //敌人数量
    private int _enemyAmount;
    //装载敌人的游戏对象列表
    private List<GameObject> enemyList;
    //等待生成间隔时间
    private WaitForSeconds waitTimeBetweenSpawns;
    //等待每波间隔时间
    private WaitForSeconds waitTimeBetweenWaves;
    //等待直到没有敌人时
    private WaitUntil waitUntillNoEnemy;
    
    protected override void Awake()
    {
        base.Awake();
        //初始化敌人列表
        enemyList = new List<GameObject>();
        //初始化等待生成间隔时间
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        //初始化等待每波间隔时间
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        //初始化等待直到没有敌人，传入敌人列表元素是否为0的匿名函数
        waitUntillNoEnemy = new WaitUntil(()=>enemyList.Count == 0);
    }
    //将Start函数改为协程，游戏开始运行时自动执行Start协程中所有内容
    IEnumerator Start()
    {
        //允许生成敌人且游戏状态不为结束状态时，循环才继续
        while (SpawnEnemy&&GameManager.GameState!=GameState.GameOver)
        {
            //场景中没有敌人时，显示波数UI
            waveUI.SetActive(true);
            //执行随机生成协程前，挂起等待每波间隔时间。新的敌人生成前，留点时间给玩家喘息，将来用于显示UI等
            yield return waitTimeBetweenWaves;
            //开始生成敌人前，关闭敌人波数UI
            waveUI.SetActive(false);
            //启用随机生成敌人协程
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    //随机生成敌人协程
    IEnumerator RandomlySpawnCoroutine()
    {
        //当前波数模上Boss战波数的余数为0，即时Boss战的波数
        if (_waveNumber % bossWaveNumber == 0)
        {
            //对象池生成一个Boss
            var boss = PoolManager.Release(bossPrefab);
            //Boss添加到敌人列表中
            enemyList.Add(boss);
        }
        else
        {
            //最小敌人数量随着波数增加而增加，波数除以整数，可有效控制最小敌人数量的增长速度。每隔一波Boss战，敌人的数量就会增加一个
            _enemyAmount = Mathf.Clamp(_enemyAmount, minEnemyAmount + _waveNumber / bossWaveNumber, maxEnemyAmount);
            //循环生成所有敌人
            for (int i = 0; i < _enemyAmount; i++)
            {
                //对象池释放敌人预制体集合中随机取出的一种敌人，每生成一个敌人就存放到敌人列表中
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
                //挂起等待一段时间
                yield return waitTimeBetweenSpawns;
            }
        }
        
        //挂起等待只到没有敌人
        yield return waitUntillNoEnemy;
        
        //当前波数中所有敌人生成完毕后，敌人波数+1
        _waveNumber++;
    }
    //将敌人从列表移除 函数
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}