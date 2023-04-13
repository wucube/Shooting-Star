using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    /// <summary>
    /// Boss血条
    /// </summary>
    private BossHealthBar _healthBar;
    //HUDѪ������Ļ������
    private Canvas _healthBarCanvas;
    protected override void Awake()
    {
        base.Awake();
        //��ȡѪ������
        _healthBar = FindObjectOfType<BossHealthBar>();
        //��ȡHUDѪ�����󻭲�
        _healthBarCanvas = _healthBar.GetComponentInChildren<Canvas>();
    }

    //��дOnEnable����
    protected override void OnEnable()
    {
        base.OnEnable();
        //��ʼ��Boss HUDѪ��
        _healthBar.Initialize(health,maxHealth);
        //����HUDѪ������
        _healthBarCanvas.enabled = true;
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
        _healthBarCanvas.enabled = false;
        base.Die();
    }

    //��д���˺���
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //boss HUDѪ����ʾ
        _healthBar.UpdateStats(health,maxHealth);
    }

    //��д����Ѫ�����ֵ������Boss���Ѫ��+���˲���*����ֵ����
    protected override void SetHealth()=>maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;

}
