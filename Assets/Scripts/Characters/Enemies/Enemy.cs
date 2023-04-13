using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Character
{
    /// <summary>
    /// 敌人的分数点
    /// </summary>
    [SerializeField] private int scorePoint = 100;

    /// <summary>
    /// 敌人死亡的能量奖励
    /// </summary>
    [SerializeField] private int deathEnergyBonus = 3;
    
    /// <summary>
    /// 血量值因子，用于计算
    /// </summary>
    [SerializeField] protected int healthFactor;

    /// <summary>
    /// 战利品生成
    /// </summary>
    private LootSpawner lootSpawner;

    protected virtual void Awake()
    {
        lootSpawner = GetComponent<LootSpawner>();
    }

    protected override void OnEnable()
    {
        SetHealth();

        base.OnEnable();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        //若敌机碰到玩家机体
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            //玩家死亡
            player.Die();
            //敌机也死亡
            Die();
        }
    }
    
    /// <summary>
    /// 敌机死亡
    /// </summary>
    public override void Die()
    {
        //添加分数点
        ScoreManager.Instance.AddScore(scorePoint);
        //玩家机体获得能量奖励
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        //敌人机体从列表中移除
        EnemyManager.Instance.RemoveFromList(gameObject);
        //生成战利品
        lootSpawner.Spawn(transform.position);

        base.Die();
    }
    /// <summary>
    /// 根据敌人波数设置敌人最大血量
    /// </summary>
    protected virtual void SetHealth() => maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
}
