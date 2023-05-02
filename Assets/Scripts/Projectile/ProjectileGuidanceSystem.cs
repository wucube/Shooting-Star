using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    //子弹类脚本引用变量
    [SerializeField] private Projectile projectile;
    //最小弹道角度
    [SerializeField] private float minBallisticAngle = -50f;
    //最大弹道角度
    [SerializeField] private float maxBallisticAngle = 50f;
    //子弹相对于目标的朝向
    private Vector3 _targetDirection;
    //弹道角度变量
    private float _ballisticAngle;
    //子弹归巢协程。归巢，军事术语中通常代指子弹导弹自动追踪功能
    public IEnumerator HomingCoroutine(GameObject target)
    {
        //循环开始前，调用随机函数为弹道角度赋值
        _ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        //当子弹处于活动状态时，则进行循环
        while (gameObject.activeSelf)
        {
            //如果目标处于活动状态，子弹持续向目标移动
            if (target.activeSelf)
            {
                //目标向量 - 子弹自身位置向量 得子弹相对目标的朝向向量
                _targetDirection = target.transform.position - transform.position;
                //转动子弹Z轴，让子弹始终朝向目标
                //用反正切函数取得朝向目标的旋转弧度，对象X轴与 对象和目标连接直线的弧度
                transform.rotation =
                    Quaternion.AngleAxis(Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg,
                        Vector3.forward);
                //修正子弹朝向后，再将子弹旋转传乘一个四元唐的欧拉角，只改Z轴角度  
                transform.rotation *= Quaternion.Euler(0f,0f,_ballisticAngle);
                //调用子弹移动函数 
                projectile.Move();
            }
            //如果目标不处于活动状态，则让子弹沿着原来设定好的移动方向前移
            else projectile.Move();
            yield return null;
        }
    }
}
