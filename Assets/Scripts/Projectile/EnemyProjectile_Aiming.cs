using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    void Awake()
    {
        //寻找玩家对象物体
        //target = GameObject.FindGameObjectWithTag("Player");
        
        //设置玩家对象为目标敌人
        SetTarget(GameObject.FindWithTag("Player"));
    }
    protected override void OnEnable()
    {
        //启用协程获取精确的移动方向
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
        
    }
    // 使用协程 获取精确的移动方向 浮点数不准确
    IEnumerator MoveDirectionCoroutine()
    {
        //挂起一帧
        yield return null;
        
        //如果玩家没有死亡，继续执行
        if (target.activeSelf)
        {
            //修改子弹移动方向，目标位置  - 当前位置 得到新移动方向 归一化移动方向，确保移动速度不奇怪
            moveDirection = (target.transform.position-transform.position).normalized;
        }
    }

}
