using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹制导系统
/// </summary>
public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    
    /// <summary>
    /// 最大弹道角度
    /// </summary>
    [SerializeField] private float minBallisticAngle = -50f;
    
    /// <summary>
    /// 最大弹道角度
    /// </summary>
    [SerializeField] private float maxBallisticAngle = 50f;
    
    /// <summary>
    /// 目标移动方向
    /// </summary>
    private Vector3 targetDirection;
    /// <summary>
    /// 弹道角度
    /// </summary>
    private float ballisticAngle;

    /// <summary>
    /// 归巢协程
    /// </summary>
    /// <param name="target">要命中的目标对象</param>
    /// <returns></returns>
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                targetDirection = target.transform.position - transform.position;
                //子弹旋转朝向目标
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                //子弹再旋转一定角度，使弹道有弧度。
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                //子弹移动
                projectile.Move();
            }
            else 
                projectile.Move();

            yield return null;
        }
    }
}
