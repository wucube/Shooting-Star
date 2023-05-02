using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    //�ӵ��Ƶ�ϵͳ�ű����ñ���  
    [SerializeField] private ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        //����׷�ٵ��˵��������Ŀ��
        SetTarget(EnemyManager.Instance.RandomEnemy);
        //�ӵ���ʼ�ƶ�ǰ�����ӵ���תֵ���û�Ĭ�ϽǶȣ���ֹ�ӵ���ת����
        transform.rotation = Quaternion.identity;
        //�ӵ�Ŀ��Ϊ�գ������ӵ�ֱ����ǰ�ƶ�����
        if (target == null) base.OnEnable();
        //�ӵ�Ŀ�겻Ϊ�գ����ӵ�׷��Ŀ��
        else StartCoroutine(guidanceSystem.HomingCoroutine(target));
    }
}
