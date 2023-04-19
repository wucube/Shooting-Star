using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//普通敌人子弹
public class EnemyProjectile : Projectile
{
    void Awake()
    {
        //如果子弹移动方向不为左，就将子弹移动方向从左转到目标方向
        if(moveDirection!=Vector2.left)
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
    }
}
