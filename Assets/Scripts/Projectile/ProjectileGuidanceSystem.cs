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
    /// 最小弹道角度
    /// </summary>
    [SerializeField] private float minBallisticAngle = -50f;
    
    /// <summary>
    /// 最大弹道角度
    /// </summary>
    [SerializeField] private float maxBallisticAngle = 50f;

    /// <summary>
    /// 朝目标移动的方向
    /// </summary>
    private Vector3 targetDirection;

    private float ballisticAngle;

    /// <summary>
    /// 归巢协程
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
       
        while (gameObject.activeSelf)
        {
           
            if (target.activeSelf)
            {
               
                targetDirection = target.transform.position - transform.position;
                //面朝目标
                transform.rotation =
                    Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg,
                        Vector3.forward);

                //增加导弹弧度
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
               
                projectile.Move();
            }
           
            else projectile.Move();

            yield return null;
        }
    }
}
