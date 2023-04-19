using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人激光
/// </summary>
public class EnemyBeam : MonoBehaviour
{
    
    [SerializeField] private float damage = 50f;
    //击中目标特效
    [SerializeField] private GameObject hitVFX;

    /// <summary>
    /// 激光碰撞体与玩家持续碰撞
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            
            player.TakeDamage(damage);
            
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
