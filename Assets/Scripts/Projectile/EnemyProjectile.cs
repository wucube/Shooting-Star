using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        //����δ��ֱ�߷�����ߵ��ӵ�����������ת�Ƕ�
         if(moveDirection!=Vector2.left)
             //Quaternion.FromToRotation() ���ݴ���Ŀ�ʼ������������򷵻�һ����תֵ
             transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
    }
}
