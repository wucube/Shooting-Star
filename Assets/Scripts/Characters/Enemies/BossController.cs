using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    //连续开火持续时间
    [SerializeField] private float continuousFireDuration = 1.5f;

    [Header("======= Player Detection ========")] 
    //检测盒的变换组件变量
    [SerializeField] private Transform playerDetectionTransform;
    //检测盒的尺寸
    [SerializeField] private Vector3 playerDetectionSize;
    //玩家层遮罩变量
    [SerializeField] private LayerMask playerLayer;

    [Header("======= Beam ========")] 
    //激光冷却时间
    [SerializeField] private float beamCooldownTime = 12f;
    //激光武器蓄力音效
    [SerializeField] private AudioData beamChargingSFX;
    //激光发射音效
    [SerializeField] private AudioData beamLaunchSFX;
    //激光武器是否冷却完毕
    private bool _isBeamReady;
    //激活动画Trigger参数转换为哈希值使用
    private int launchBeamID = Animator.StringToHash("launchBeam");
    //最小开火间隔时间
    private WaitForSeconds waitForContinuousFireInterval;
    
    private WaitForSeconds waitForFireInterval;
    //等待激光冷却时间
    private WaitForSeconds _waitBeamCooldownTime;

    //子弹对象列表 弹匣
    private List<GameObject> magazine;
    //存放开火音效
    private AudioData launchSFX;
    //动画组件
    private Animator _animator;
    //玩家位置
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        //取得boss预制体上的动画器组件
        _animator = GetComponent<Animator>();
        
        //初始化最小开火间隔时间
        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        //初始化最长开火间隔时间
        waitForFireInterval = new WaitForSeconds(maxFireInterval);
        //等待激光冷却时间初始化
        _waitBeamCooldownTime = new WaitForSeconds(beamCooldownTime);
        //初始化弹匣列表，传入子弹数组长度控制列表容量
        magazine = new List<GameObject>(projectiles.Length);
        //初始化玩家位置信息，取得玩家的变换组件
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //重写OnEnable函数
    protected override void OnEnable()
    {
        //激光没准备好
        _isBeamReady = false;
        //停止播放枪口特效
        muzzleVFX.Stop();
        //启用激光冷却协程
        StartCoroutine(nameof(BeamCooldownCoroutine));
        //调用基类函数
        base.OnEnable();
    }

    //画出Boss检测盒的范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }

    //激光武器的启用与发射
    void ActivateBeamWeapon()
    {
        //激光武器没有准备好
        _isBeamReady = false;
        //通知动画器播放发射激光的动画
        _animator.SetTrigger(launchBeamID);
        //播放激光武器蓄力音效
        AudioManager.Instance.PlayerRandomSFX(beamChargingSFX);
    }

    //动画事件函数，发射激光时播放发射音效
    void AnimationEventLaunchBeam()
    {
        AudioManager.Instance.PlayerRandomSFX(beamLaunchSFX);
    }

    //动画事件函数，停止发射激光
    void AnimationEventStopBeam()
    {
        //停止追击玩家协程
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        //再次开启激光武器的冷却时间
        StartCoroutine(nameof(BeamCooldownCoroutine));
        //再次开启被关闭的随机开火协程
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }
    //装填子弹
    void LoadProjectiles()
    {
        //先清空子弹列表
        magazine.Clear();
        //玩家机体在Boss正前方时
        if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))
        {
            //装填1号子弹
            magazine.Add(projectiles[0]);
            //播放子弹发射音效
            launchSFX = projectileLaunchSFX[0];
        }
        else //玩家机体不在Boss正前方
        {
            if (Random.value < 0.5f)
            {
                //装填2号子弹
                magazine.Add(projectiles[1]);
                launchSFX = projectileLaunchSFX[1];
            }
            else
            {
                //装填所有不同角度的3号子弹
                for (int i = 2; i < projectiles.Length; i++)
                    magazine.Add(projectiles[i]);
                
                launchSFX = projectileLaunchSFX[2];
            }
        }
    }

    //重写随机开火协程
    protected override IEnumerator RandomlyFireCoroutine()
    {
        //boss处于活动状态，就一直循环
        while (isActiveAndEnabled)
        {
            //游戏结束时，停止协程
            if (GameManager.GameState == GameState.GameOver) yield break;
            //如果激光武器已冷却完毕
            if (_isBeamReady)
            {
                //激活激光武器
                ActivateBeamWeapon();
                //开启追踪玩家协程
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                //停止开火协程
                yield break;
            }
            //挂起等待一段时间
            yield return waitForFireInterval;
            //挂起执行连续开火协程
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    //连续开火协程
    IEnumerator ContinuousFireCoroutine()
    {
        //调用装填子弹函数
        LoadProjectiles();
        //播放枪口特效
        muzzleVFX.Play();
        
        //持续开火计时器
        float continuousFireTimer = 0f;
        //持续开火计时器 小于 连续开火持续时间，就一直开火
        while (continuousFireTimer < continuousFireDuration)
        {
            //发射弹匣中的子弹
            foreach (var projectile in magazine)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            //计时器增加开火间隔时间值
            continuousFireTimer += minFireInterval;
            //播放开火音效
            AudioManager.Instance.PlayerRandomSFX(launchSFX);
            //挂起等待最小开火间隔时间
            yield return waitForContinuousFireInterval;
        }
        //停止播放枪口特效
        muzzleVFX.Stop();
    }

    //激光武器冷却协程
    IEnumerator BeamCooldownCoroutine()
    {
        //等待挂起激光冷却时间
        yield return _waitBeamCooldownTime;
        //激光已准备完成
        _isBeamReady = true;
    }

    //追击玩家协程
    IEnumerator ChasingPlayerCoroutine()
    {
        //Boss激活时一直循环
        while (isActiveAndEnabled)
        {
            //修改Boss移动时的目标位置
            //X轴为画面最右边 减去 Boss模型的x边距值
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            //Y轴为玩家位置的Y轴
            targetPosition.y = playerTransform.position.y;
            yield return null;
        }
    }
}
