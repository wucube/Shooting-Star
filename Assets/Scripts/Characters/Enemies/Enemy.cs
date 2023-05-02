using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Character
{
    //?????????????��????
    [SerializeField] private int scorePoint = 100;
    //????????????
    [SerializeField] private int deathEnergyBonus = 3;
    
    //Ѫ������
    [SerializeField] protected int healthFactor;

    //ս��Ʒ�������ű�
    private LootSpawner lootSpawner;

    protected virtual void Awake()
    {
        //ȡ��ս��Ʒ������
        lootSpawner = GetComponent<LootSpawner>();
    }

    protected override void OnEnable()
    {
        //���õ������Ѫ��
        SetHealth();
        base.OnEnable();
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        //????��?????????
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            //???????
            player.Die();
            //?��?????
            Die();
        }
    }
    //��д��ɫ��������
    public override void Die()
    {
        //????????????��?
        ScoreManager.Instance.AddScore(scorePoint);
        //????????????????????????????????????
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        //??????��???????????????
        EnemyManager.Instance.RemoveFromList(gameObject);
        //���������������ɺ���
        lootSpawner.Spawn(transform.position);
        
        //????????????????
        base.Die();
    }
    //���õ������Ѫ�����������Ѫ�����ϵ��˲�����ص���ֵ
    protected virtual void SetHealth() => maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
}
