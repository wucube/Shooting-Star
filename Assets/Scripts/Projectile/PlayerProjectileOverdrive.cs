using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家能量爆发时的子弹
/// </summary>
public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] private ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        //设置子弹的目标
        SetTarget(EnemyManager.Instance.RandomEnemy);
        //重置子弹的旋转角度，避免从对象池取出的子弹还保留着上次的旋转角度
        transform.rotation = Quaternion.identity;
        
        if (target == null)
            base.OnEnable();
        else
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
    }
}
