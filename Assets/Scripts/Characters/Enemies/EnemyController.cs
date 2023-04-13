using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 敌人控制器
/// </summary>
public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 敌机X轴的边距
    /// </summary>   
    [Header("---- MOVE ----")]
    protected float paddingX;

    /// <summary>
    /// 敌机Y轴的边距
    /// </summary>
    float paddingY;

    /// <summary>
    /// 敌机移动速度
    /// </summary>
    [SerializeField] float moveSpeed = 2f;

    /// <summary>
    /// 敌机上下移动时旋转的角度
    /// </summary>
    [SerializeField] float moveRotationAngle = 25f;
    
    /// <summary>
    /// 子弹对象数组
    /// </summary>
    [Header("---- FIRE ----")]
    [SerializeField] protected GameObject[] projectiles;
    
    /// <summary>
    /// 发射子弹的音频数据数组
    /// </summary>
    [SerializeField] protected AudioData[] projectileLaunchSFX;
    
    /// <summary>
    /// 枪口位置
    /// </summary>
    [SerializeField] protected Transform muzzle;
    
    /// <summary>
    /// 枪口粒子特效
    /// </summary>
    [SerializeField] protected ParticleSystem muzzleVFX;
    
    /// <summary>
    /// 最小开火间隔
    /// </summary>
    [SerializeField] protected float minFireInterval;

    /// <summary>
    /// 最大开火间隔
    /// </summary>
    [SerializeField] protected float maxFireInterval;
    
    /// <summary>
    /// 移动到目标位置
    /// </summary>
    protected Vector3 targetPosition;
    
    /// <summary>
    /// 固定帧更新的等待时长
    /// </summary>
    /// <returns></returns>
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        //得到机体模型矩形范围
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;

        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }
    
    protected virtual void OnEnable()
    {

        StartCoroutine(nameof(RandomlyMovingCoroutine));

        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    protected virtual void OnDisable()
    {
        //停止所有协程
        StopAllCoroutines();
    }

    /// <summary>
    /// 随机移动协程
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator RandomlyMovingCoroutine() 
    {
        
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        //若敌机处于活动状态
        while (gameObject.activeSelf)
        {
            //敌机位置与目标位置的距离 大于等于 敌机每固定帧时间的距离距离
            if (Vector3.Distance(transform.position, targetPosition) >=moveSpeed * Time.fixedDeltaTime)
            {
                //敌机朝目标位置匀速移动
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                //敌机移动时沿世界坐标X轴旋转，越靠近垂直方向移动，旋转角度越大。
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else //否则目标位置重新赋值
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX,paddingY);
             
            yield return new WaitForFixedUpdate();
        }
    }
    
    /// <summary>
    /// 随机开火协程
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            //在最大或最小开火间隔之间后开火
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            
            //游戏结束则直接退出开火协程
            if(GameManager.GameState ==GameState.GameOver) yield break;
            
            foreach (var projectile in projectiles)
            {   
                //对象池释放子弹
                PoolManager.Release(projectile, muzzle.position);
            }

            AudioManager.Instance.PlayerRandomSFX(projectileLaunchSFX);
            muzzleVFX.Play();
        }
    }
}
