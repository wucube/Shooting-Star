using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    //子弹制导系统脚本引用变量  
    [SerializeField] private ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        //设置追踪敌人的随机敌人目标
        SetTarget(EnemyManager.Instance.RandomEnemy);
        //子弹开始移动前，将子弹旋转值重置回默认角度，防止子弹旋转混乱
        transform.rotation = Quaternion.identity;
        //子弹目标为空，则让子弹直接往前移动即可
        if (target == null) base.OnEnable();
        //子弹目标不为空，则子弹追踪目标
        else StartCoroutine(guidanceSystem.HomingCoroutine(target));
    }
}
