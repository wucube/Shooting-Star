using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    //敌机中心点到边缘X、Y轴的偏差值 
    protected float paddingX;
     float _paddingY;
    //敌人移动速度
    [SerializeField] float moveSpeed = 2f;
    //敌人移动旋转变量
    [SerializeField] float moveRotationAngle = 25f;

    [Header("---- FIRE ----")]
    //敌人子弹命中视觉特效对象集合
    [SerializeField] protected GameObject[] projectiles;
    //敌人发射子弹的音频数据集合
    [SerializeField] protected AudioData[] projectileLaunchSFX;
    
    //开火位置 枪口
    [SerializeField] protected Transform muzzle;
    
    //枪口特效变量
    [SerializeField] protected ParticleSystem muzzleVFX;
    
    //最小开火间隔时间
    [SerializeField] protected float minFireInterval;
    //最大开火间隔时间
    [SerializeField] protected float maxFireInterval;
    
    //敌人目标移动位置
    protected Vector3 targetPosition;
    
    //每帧最大移动距离
    //private float _maxMoveDistancePerFrame;
    //挂起等待下一个固定帧
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        //初始化每帧最大移动距离
        //_maxMoveDistancePerFrame = moveSpeed * Time.fixedDeltaTime;
        //通过模型对象的渲染器组件得到模型的尺寸值
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        _paddingY = size.y / 2f;
    }
    
    //敌机启用一次就调用一次
    protected virtual void OnEnable()
    {
        //启用随机移动协程
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        //随机开火
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }
    //游戏停运时调用
    protected virtual void OnDisable()
    {
        //停用所有协程
        StopAllCoroutines();
    }
    //敌人随机移动协程
    protected virtual IEnumerator RandomlyMovingCoroutine() 
    {
        //敌机在最右镜头外的随机位置
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, _paddingY);
        //敌机的目标位置，在镜头内的右侧随机位置
        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, _paddingY);
        //如果敌机没有被摧毁则一直循环
        while (gameObject.activeSelf)
        {
            //如果敌机未到达目标位置 即敌机当前位置与目标位置距离 大于 敌机每帧移动的距离
            if (Vector3.Distance(transform.position, targetPosition) >=moveSpeed * Time.fixedDeltaTime)
            {
                //继续前往目标位置 以每帧移动距离 前往目标位置
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                //敌人移动时旋转 取得移动方向向量，归一化后Y轴值 乘 旋转角度，沿敌人对象的X轴正方向旋转
                transform.rotation =
                    Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle,
                        Vector3.right);
            }
            //敌机如果已到达目标位置，则给予一个新的目标位置
            else 
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX,_paddingY);
            //挂起直到下一个固定帧
            yield return new WaitForFixedUpdate();
        }
    }
    
    //敌人随机开火协程
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        //如果敌机没有摧毁则一起循环
        while(gameObject.activeSelf)
        {
            //在最大、最小开火间隔中，随机开火。只能在循环中新建变量，才能完全实现每次随机时间开火
            yield return new WaitForSeconds(Random.Range(minFireInterval,maxFireInterval));
            
            //如果游戏状态为结束，则结束协程
            if(GameManager.GameState ==GameState.GameOver) yield break;
            
            foreach (var projectile in projectiles)
            {
                //对象池生成数组中的所有子弹
                PoolManager.Release(projectile, muzzle.position);
            }
            //播放随机子弹发射音效
            AudioManager.Instance.PlayerRandomSFX(projectileLaunchSFX);
            //播放枪口特效
            muzzleVFX.Play();
        }
    }
}
