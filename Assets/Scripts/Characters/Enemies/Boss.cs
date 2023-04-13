using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    /// <summary>
    /// Boss血条
    /// </summary>
    private BossHealthBar healthBar;

    /// <summary>
    /// Boss血条UI的画布
    /// </summary>
    private Canvas healthBarCanvas;

    protected override void Awake()
    {
        base.Awake();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health,maxHealth);
        healthBarCanvas.enabled = true;
    }
    
    protected override void OnCollisionEnter2D(Collision2D other)
    {
       //如果Boss碰到玩家，玩家直接死亡
       if(other.gameObject.TryGetComponent<Player>(out Player player))
           player.Die();
    }

    /// <summary>
    /// Boss死亡
    /// </summary>
    public override void Die()
    {
        //Boss血条隐藏
        healthBarCanvas.enabled = false;
        base.Die();
    }

    /// <summary>
    /// Boss受伤
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateStats(health,maxHealth);
    }

    /// <summary>
    /// 根据敌人波数设置Boss的血量
    /// </summary>
    protected override void SetHealth()=>maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;

}
