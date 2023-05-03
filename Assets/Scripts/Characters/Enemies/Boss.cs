using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //Boss HUD血条变量
    private BossHealthBar _healthBar;
    //HUD血条对象的画布组件
    private Canvas _healthBarCanvas;
    protected override void Awake()
    {
        base.Awake();
        //获取血条对象
        _healthBar = FindObjectOfType<BossHealthBar>();
        //获取HUD血条对象画布
        _healthBarCanvas = _healthBar.GetComponentInChildren<Canvas>();
    }

    //重写OnEnable函数
    protected override void OnEnable()
    {
        base.OnEnable();
        //初始化Boss HUD血条
        _healthBar.Initialize(health,maxHealth);
        //开启HUD血条画布
        _healthBarCanvas.enabled = true;
    }
    
    //重写敌人类的碰撞函数
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        //玩家撞上Boss机
       if(other.gameObject.TryGetComponent<Player>(out Player player))
           //只有玩家会死掉
           player.Die();
    }

    //重写死亡函数
    public override void Die()
    {
        //关闭boss HUD画布
        _healthBarCanvas.enabled = false;
        base.Die();
    }

    //重写受伤函数
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //boss HUD血条显示
        _healthBar.UpdateStats(health,maxHealth);
    }

    //重写设置血量最大值函数，Boss最大血量+敌人波数*健康值因素
    protected override void SetHealth()=>maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;

}
