using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss控制器
/// </summary>
public class BossController : EnemyController
{
    /// <summary>
    /// 连续开火的持续时间
    /// </summary>
    [SerializeField] private float continuousFireDuration = 1.5f;

    
    /// <summary>
    /// 玩家检测器的位置
    /// </summary>
    [Header("======= Player Detection ========")] 
    [SerializeField] private Transform playerDetectionTransform;

    /// <summary>
    /// 玩家检测器的尺寸
    /// </summary>
    [SerializeField] private Vector3 playerDetectionSize;

    /// <summary>
    /// 玩家检测层
    /// </summary>
    [SerializeField] private LayerMask playerLayer;

    /// <summary>
    /// 激光冷却时间
    /// </summary>
    [Header("======= Beam ========")] 
    [SerializeField] private float beamCooldownTime = 12f;

    /// <summary>
    /// 激光充能音效
    /// </summary>
    [SerializeField] private AudioData beamChargingSFX;

    /// <summary>
    /// 激光发射音效
    /// </summary>
    [SerializeField] private AudioData beamLaunchSFX;

    /// <summary>
    /// 激光是否准备完成
    /// </summary>
    private bool isBeamReady;

    /// <summary>
    /// 激光动画名的哈希值
    /// </summary>
    /// <returns></returns>
    private int launchBeamID = Animator.StringToHash("launchBeam");

    /// <summary>
    /// 持续开火的间隔
    /// </summary>
    private WaitForSeconds waitForContinuousFireInterval;
    
    /// <summary>
    /// 开火间隔
    /// </summary>
    private WaitForSeconds waitForFireInterval;

    private WaitForSeconds waitBeamCooldownTime;

    /// <summary>
    /// 弹匣
    /// </summary>
    private List<GameObject> magazine;

    /// <summary>
    /// 发射声效
    /// </summary>
    private AudioData launchSFX;
    
    private Animator animator;

    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        
        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);

        waitForFireInterval = new WaitForSeconds(maxFireInterval);

        waitBeamCooldownTime = new WaitForSeconds(beamCooldownTime);

        magazine = new List<GameObject>(projectiles.Length);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        isBeamReady = false;

        muzzleVFX.Stop();

        StartCoroutine(nameof(BeamCooldownCoroutine));

        base.OnEnable();
    }

    /// <summary>
    /// 绘制检测玩家的范围
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }

    /// <summary>
    /// 启动激光武器
    /// </summary>
    void ActivateBeamWeapon()
    {
        //下次的激光还未准备完成
        isBeamReady = false;
        
        //播放激光发射动画
        animator.SetTrigger(launchBeamID);

        AudioManager.Instance.PlayerRandomSFX(beamChargingSFX);
    }

    /// <summary>
    /// 激光发射动画的事件处理函数
    /// </summary>
    void AnimationEventLaunchBeam()
    {
        AudioManager.Instance.PlayerRandomSFX(beamLaunchSFX);
    }

    /// <summary>
    /// 激光停止发射的动画事件处理函数
    /// </summary>
    void AnimationEventStopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));

        StartCoroutine(nameof(BeamCooldownCoroutine));

        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    /// <summary>
    /// 填装子弹
    /// </summary>
    void LoadProjectiles()
    {
        //弹匣清空
        magazine.Clear();

        //检测到玩家机体
        if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))
        {
            //添加子弹
            magazine.Add(projectiles[0]);

            launchSFX = projectileLaunchSFX[0];
        }
        else //没有检测到玩家机体就随机填装子弹
        {
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);

                launchSFX = projectileLaunchSFX[1];
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                    magazine.Add(projectiles[i]);
                
                launchSFX = projectileLaunchSFX[2];
            }
        }
    }

    /// <summary>
    /// 随机开火协程
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator RandomlyFireCoroutine()
    {
        while (isActiveAndEnabled)
        {
            //若游戏结束，退出开火协程
            if (GameManager.GameState == GameState.GameOver) yield break;
            
            //若激光准备完成
            if (isBeamReady)
            {
                ActivateBeamWeapon();

                StartCoroutine(nameof(ChasingPlayerCoroutine));

                yield break;
            }

            yield return waitForFireInterval;

            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    //持续开火协程
    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();

        muzzleVFX.Play();
        
        //连续开火计时器
        float continuousFireTimer = 0f;

        //若连续开火计时 小于 连续开火持续时间
        while (continuousFireTimer < continuousFireDuration)
        {
            //对象池挨个释放子弹
            foreach (var projectile in magazine)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            continuousFireTimer += minFireInterval;

            AudioManager.Instance.PlayerRandomSFX(launchSFX);
  
            yield return waitForContinuousFireInterval;
        }

        muzzleVFX.Stop();
    }
    
    /// <summary>
    /// 激光冷却协程
    /// </summary>
    /// <returns></returns>
    IEnumerator BeamCooldownCoroutine()
    {

        yield return waitBeamCooldownTime;
  
        isBeamReady = true;
    }

    /// <summary>
    /// 追踪玩家机体的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = Viewport.Instance.MaxX - paddingX;

            targetPosition.y = playerTransform.position.y;
            
            yield return null;
        }
    }
}
