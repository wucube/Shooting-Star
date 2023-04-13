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
    
    //��д���������ײ����
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        //���ײ��Boss��
       if(other.gameObject.TryGetComponent<Player>(out Player player))
           //ֻ����һ�����
           player.Die();
    }

    //��д��������
    public override void Die()
    {
        //�ر�boss HUD����
        healthBarCanvas.enabled = false;
        base.Die();
    }

    //��д���˺���
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //boss HUDѪ����ʾ
        healthBar.UpdateStats(health,maxHealth);
    }

    //��д����Ѫ�����ֵ������Boss���Ѫ��+���˲���*����ֵ����
    protected override void SetHealth()=>maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;

}
