using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Character
{
    [SerializeField] private int scorePoint = 100;
    [SerializeField] private int deathEnergyBonus = 3;
    
    //血量因素
    [SerializeField] protected int healthFactor;

    //战利品生成器脚本
    private LootSpawner lootSpawner;

    protected virtual void Awake()
    {
        //取得战利品生成器
        lootSpawner = GetComponent<LootSpawner>();
    }

    protected override void OnEnable()
    {
        //设置敌人最大血量
        SetHealth();
        base.OnEnable();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }
    //重写角色死亡函数
    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        lootSpawner.Spawn(transform.position);
        base.Die();
    }
    //设置敌人最大血量函数，最大血量加上敌人波数相关的数值
    protected virtual void SetHealth() => maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
}
