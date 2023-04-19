using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家普通子弹
/// </summary>
public class PlayerProjectile :Projectile
{
    /// <summary>
    /// 子弹轨迹
    /// </summary>
    private TrailRenderer trail;
    protected virtual void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        
        //子弹移动方向修正
        if (moveDirection != Vector2.right)
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
    }
    void OnDisable()
    {
        trail.Clear();
    }
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        PlayerEnergy.Instance.Obtain(PlayerEnergy.Percent);
    }
}
