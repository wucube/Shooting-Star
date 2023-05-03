using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家能量爆发子弹
/// </summary>
public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] private ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
       
        SetTarget(EnemyManager.Instance.RandomEnemy);

        //子弹从对象池取出时重置旋转角度
        transform.rotation = Quaternion.identity;
        
        if (target == null) base.OnEnable();
       
        else StartCoroutine(guidanceSystem.HomingCoroutine(target));
    }
}
