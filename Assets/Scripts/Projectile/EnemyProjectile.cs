using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        //修正未以直线飞往左边的子弹，修正其旋转角度
         if(moveDirection!=Vector2.left)
             //Quaternion.FromToRotation() 根据传入的开始与结束两个方向返回一个旋转值
             transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
    }
}
