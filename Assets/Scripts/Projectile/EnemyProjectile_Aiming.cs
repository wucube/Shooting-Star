using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人的追踪子弹
/// </summary>
public class EnemyProjectile_Aiming : Projectile
{
    void Awake()
    {
        //target = GameObject.FindGameObjectWithTag("Player");

        //将追踪目标设置为敌人
        SetTarget(GameObject.FindWithTag("Player"));
    }
    protected override void OnEnable()
    {
        
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
        
    }
    
    /// <summary>
    /// 持续改变子弹移动方向的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;

        //如果目标激活
        if (target.activeSelf)
        {
            //目标位置 - 子弹自身位置 得到的向量归一化就是子弹移动方向
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }

}
