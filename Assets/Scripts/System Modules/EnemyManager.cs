using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    //���ȡ��һ��Ŀ����˵�����
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    //��ȡ���˲�����ֵ������
    public int WaveNumber => _waveNumber;
    //��ȡÿ�����ʱ�������
    public float TimeBetweenWaves => timeBetweenWaves;
    //�Ƿ����ɵ���
    [SerializeField] private bool SpawnEnemy = true;
    //�洢����UI����
    [SerializeField] private GameObject waveUI;
    //����Ԥ��������
    [SerializeField] private GameObject[] enemyPrefabs;
    //�������ɼ��ʱ��
    [SerializeField] private float timeBetweenSpawns = 1f;
    //�ȴ���һ��ʱ��
    [SerializeField] private float timeBetweenWaves = 1f;
    //��С��������
    [SerializeField] private int minEnemyAmount = 4;
    //����������
    [SerializeField] private int maxEnemyAmount = 10;
    
    [Header("======= Boss Settings =======")]
    //BossԤ�������
    [SerializeField] private GameObject bossPrefab;
    //Bossս������ֵ����
    [SerializeField] private int bossWaveNumber;
    
    //���˲���������Ĭ��Ϊ1
    private int _waveNumber = 1;
    //��������
    private int _enemyAmount;
    //װ�ص��˵���Ϸ�����б�
    private List<GameObject> enemyList;
    //�ȴ����ɼ��ʱ��
    private WaitForSeconds waitTimeBetweenSpawns;
    //�ȴ�ÿ�����ʱ��
    private WaitForSeconds waitTimeBetweenWaves;
    //�ȴ�ֱ��û�е���ʱ
    private WaitUntil waitUntillNoEnemy;
    
    protected override void Awake()
    {
        base.Awake();
        //��ʼ�������б�
        enemyList = new List<GameObject>();
        //��ʼ���ȴ����ɼ��ʱ��
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        //��ʼ���ȴ�ÿ�����ʱ��
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        //��ʼ���ȴ�ֱ��û�е��ˣ���������б�Ԫ���Ƿ�Ϊ0����������
        waitUntillNoEnemy = new WaitUntil(()=>enemyList.Count == 0);
    }
    //��Start������ΪЭ�̣���Ϸ��ʼ����ʱ�Զ�ִ��StartЭ������������
    IEnumerator Start()
    {
        //�������ɵ�������Ϸ״̬��Ϊ����״̬ʱ��ѭ���ż���
        while (SpawnEnemy&&GameManager.GameState!=GameState.GameOver)
        {
            //������û�е���ʱ����ʾ����UI
            waveUI.SetActive(true);
            //ִ���������Э��ǰ������ȴ�ÿ�����ʱ�䡣�µĵ�������ǰ������ʱ�����Ҵ�Ϣ������������ʾUI��
            yield return waitTimeBetweenWaves;
            //��ʼ���ɵ���ǰ���رյ��˲���UI
            waveUI.SetActive(false);
            //����������ɵ���Э��
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    //������ɵ���Э��
    IEnumerator RandomlySpawnCoroutine()
    {
        //��ǰ����ģ��Bossս����������Ϊ0����ʱBossս�Ĳ���
        if (_waveNumber % bossWaveNumber == 0)
        {
            //���������һ��Boss
            var boss = PoolManager.Release(bossPrefab);
            //Boss��ӵ������б���
            enemyList.Add(boss);
        }
        else
        {
            //��С�����������Ų������Ӷ����ӣ�������������������Ч������С���������������ٶȡ�ÿ��һ��Bossս�����˵������ͻ�����һ��
            _enemyAmount = Mathf.Clamp(_enemyAmount, minEnemyAmount + _waveNumber / bossWaveNumber, maxEnemyAmount);
            //ѭ���������е���
            for (int i = 0; i < _enemyAmount; i++)
            {
                //������ͷŵ���Ԥ���弯�������ȡ����һ�ֵ��ˣ�ÿ����һ�����˾ʹ�ŵ������б���
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
                //����ȴ�һ��ʱ��
                yield return waitTimeBetweenSpawns;
            }
        }
        
        //����ȴ�ֻ��û�е���
        yield return waitUntillNoEnemy;
        
        //��ǰ���������е���������Ϻ󣬵��˲���+1
        _waveNumber++;
    }
    //�����˴��б��Ƴ� ����
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}