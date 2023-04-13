using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    //�����˺�ֵ
    [SerializeField] private float damage = 50f;
    //����;��ʱ���Ӿ���Ч
    [SerializeField] private GameObject hitVFX;

     void OnCollisionStay2D(Collision2D collision)
    {
        //����ײ�������ܻ�ȡ����ҽű�
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            //����������˺���
            player.TakeDamage(damage);
            //��������������Ӿ���Ч
            PoolManager.Release(hitVFX, collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
