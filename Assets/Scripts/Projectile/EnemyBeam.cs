using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    //激光伤害值
    [SerializeField] private float damage = 50f;
    //激光途中时的视觉特效
    [SerializeField] private GameObject hitVFX;

     void OnCollisionStay2D(Collision2D collision)
    {
        //若碰撞对象上能获取到玩家脚本
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            //调用玩家受伤函数
            player.TakeDamage(damage);
            //对象池生成命中视觉特效
            PoolManager.Release(hitVFX, collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
