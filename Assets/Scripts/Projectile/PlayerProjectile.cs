using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile :Projectile
{
    private TrailRenderer _trail;
    protected virtual void Awake()
    {
       
        _trail = GetComponentInChildren<TrailRenderer>();
        
        if (moveDirection != Vector2.right)
            
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
    }
    
    void OnDisable()
    {
        _trail.Clear();
    }
  
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        PlayerEnergy.Instance.Obtain(PlayerEnergy.Percent);
    }
}
