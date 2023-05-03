using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    //HUD血条变量
    [SerializeField] StatsBar_HUD statsBar_HUD;
    //是否再生生命值
    [SerializeField] bool regenerateHealth = true;
    //生命值再生时间
    [SerializeField] float healthRegenerateTime;
    //生命值再生百分比
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;
    
    [Header("------ INPUT -------")] 
    //玩家输入类的引用变量
    [SerializeField] PlayerInput input;

    [Header("------ MOVE -------")]
    //机体移动速度
    [SerializeField] float moveSpeed = 10f;
    //机体边缘距机体中心点的距离
    private float _paddingX;
    private float _paddingY;
    
    //加速时间
    [SerializeField] float accelerationTime = 3f;
    //减速时间
    [SerializeField] float decelerationTime = 3f;
    //机体旋转角度
    [SerializeField] float moveRotationAngle = 50f;

    //玩家血量是否全满属性
    public bool IsFullHealth => health == maxHealth;
    //玩家武器威力是否满级属性
    public bool IsFullPower => weaponPower == 2;

    [FormerlySerializedAs("projectile")]
    [Header("------ FIRE -------")]
    //子弹物体
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    //能量爆发子弹预制体
    [SerializeField] private GameObject projectileOverdrive;
    
    //枪口特效
    [SerializeField] private ParticleSystem muzzleVFX;
    
    //子弹生成位置  枪口
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    //子弹发射的音频数据变量
    [SerializeField] AudioData projectileSFX;
    //武器威力
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    
    //开火间隔
    [SerializeField] float fireInterval = 0.2f;

    [Header("------ DODGE -------")] 
    //闪避音频数据
    [SerializeField] private AudioData dodgeSFX;
    //闪避能量消耗值
    [SerializeField, Range(0, 100)] private int dodgeEnergyCost = 25;
    //最大滚转角
    [SerializeField] private float maxRoll = 360f;
    //翻滚速度
    [SerializeField] private float rollSpeed = 360f;
    //闪避时模型缩放最小值
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);


    [Header("------ OVERDRIVE -------")]
    //能量爆发时的闪避因数变量
    [SerializeField] private int overdriveDodgeFactor = 2;
    //能量爆发时的速度因数变量
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    //能量爆发时的开火因数变量
    [SerializeField] private float overdriveFireFactor = 1.2f;
   
    //玩家无敌持续时间
    readonly float InvincibleTime = 1f;
    
    //是否正在闪避中
    private bool _isDodging = false;
    //玩家是否处于能量爆发
    private bool _isOverdriving = false;
    //子弹时间恢复过程时间，只读变量
    private readonly float slowMotionDuration = 1f;
    
    //存储当前的滚转角
    private float _currentRoll;
    //闪避持续时间
    private float _dodgeDuration;
    
    //记录协程循环中时间
    private float _t;
    //玩家移动方向
    private Vector2 _moveDirection;
    //用于记录玩家刚体初始速度
    private Vector2 _previousVelocity;
    //用于记录玩家初始四元数
    private Quaternion _previousRotation;
    
    //协程的子弹生成等待挂起时间
    WaitForSeconds waitForFireInterval;
    //等待能量爆发时的开火间隔
    WaitForSeconds waitForOverdriveFireInterval;
    //等待生命值再生时间
    WaitForSeconds waitHealthRegenerateTime;
    //等待减速时间
    WaitForSeconds waitDecelerationTime;
    //等待无敌时间
    private WaitForSeconds waitInvincibleTime;
    //等待下一次固定帧更新
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    //用于存储带参数的移动协程函数的变量
    Coroutine moveCoroutine;
    //生命值恢复协程变量
    Coroutine healthRegenerateCoroutine;

    Rigidbody2D _rigidbody;
    //2D碰撞体变量
    private  Collider2D _collider;
     
    //导弹系统变量
    private MissileSystem _missile;
    
    void Awake()
    {
        //获取刚体组件的引用
        _rigidbody = GetComponent<Rigidbody2D>();
        //获取玩家碰撞体组件的引用
        _collider = GetComponent<Collider2D>();
        //取得导弹脚本系统实例
        _missile = GetComponent<MissileSystem>();
        
        //通过模型对象的渲染器组件取得模型尺寸
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        //模型中心到边距值 为模型长宽的一半
        _paddingX = size.x / 2f;
        _paddingY = size.y / 2f;
        
        //初始化闪避时间
        _dodgeDuration = maxRoll / rollSpeed;
        
        _rigidbody.gravityScale = 0f;
        //初始化等待能量爆发的开火时间间隔，参数为 开火间隔时间 除以 能量爆发开火因数，实际开火间隔时间缩短
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval/=overdriveFireFactor);
        
        waitForFireInterval = new WaitForSeconds(fireInterval);
        //初始化生命值再生时间
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        //初始化等待减速时间，传入玩家减速时间
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
        
        //初始化等待无敌时间
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);
    }
    
    //函数重写
    protected override void OnEnable()
    {
        base.OnEnable();
        //玩家输入类的移动和停止移动的事件订阅对应处理函数
        input.onMove += Move;
        input.onStopMove += StopMove;
        //玩家输入类的开火停火事件订阅对应事件处理器
        input.onFire += Fire;
        input.onStopFire += StopFire;
        //订阅玩家输入类的闪避事件
        input.onDodge += Dodge;
        //订阅输入管理类的能量爆发事件
        input.onOverdrive += Overdrive;
        //订阅发射导弹事件
        input.onLaunchMissile += LaunchMissile;
        //订阅能量爆发系统的开启与关闭委托
        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }
    void OnDisable()
    {
        //玩家输入类的移动和停止移动事件退订对应事件处理器
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        //玩家输入类的开火停火事件退订对应事件处理器
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        //退订玩家输入类的闪避事件
        input.onDodge -= Dodge;
        //退订能量爆发事件
        input.onOverdrive -= Overdrive;
        //退订发射导弹事件
        input.onLaunchMissile -= LaunchMissile;
        //退订能量爆发系统的开启关闭委托
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    // Start is called before the first frame update
    void Start()
    {
        //修改刚体的重力缩放值为0，不置重力为0， 游戏运行时主角刚坠机 
        _rigidbody.gravityScale = 0f;
        
        //初始化子弹生成等待挂起时间
        waitForFireInterval = new WaitForSeconds(fireInterval);
        //初始化回血等待挂起时间
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        
        //初始化HUD血条
        statsBar_HUD.Initialize(health, maxHealth);
        
        //Player脚本运行时，激活Gameplay动作表
        input.EnableGameplayInput();
    }

    //重写受伤函数
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //玩家受伤时武器威力下降
        PowerDown();
        
        //更新HUD血条状态
        statsBar_HUD.UpdateStats(health, maxHealth);
        //调用子弹时间函数
        TimeController.Instance.BulletTime(slowMotionDuration);
        
        //玩家游戏对象处于活动状态
        if (gameObject.activeSelf)
        {
            //受伤时调用Move函数
            Move(_moveDirection);
            //开启玩家无敌时间协程
            StartCoroutine(InvincibleCoroutine());
            
            //生命再生开关开启
            if (regenerateHealth)
            {
                //如果再生协程不为空，先停用再生协程，确保每次调用协程时，先前的回血协程已经停用
                if (healthRegenerateCoroutine != null)
                    StopCoroutine(healthRegenerateCoroutine);
                //启用生命再生协程，传入再生时间与生命恢复百分比量
                healthRegenerateCoroutine =
                    StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }
    //重写回血函数
    public override void RestoreHealth(float value)
    {
        //调用基类函数
        base.RestoreHealth(value);
        //更新血条状态
        statsBar_HUD.UpdateStats(health, maxHealth);
    }
    //重写死亡函数
    public override void Die()
    {
        //调用游戏管理器的游戏结束委托
        GameManager.onGameOver?.Invoke();
        //游戏状态为结束
        GameManager.GameState = GameState.GameOver;
        
        //先更新血条状态，当前状态值为0，血条清空
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    //无敌时间协程
    IEnumerator InvincibleCoroutine()
    {
        //玩家碰撞体触发器开启一段时间后再关闭
        _collider.isTrigger = true;
        yield return waitInvincibleTime;
        _collider.isTrigger = false;
    }
    #region MOVE

    //移动事件处理函数
    void Move(Vector2 moveInput)
    {
        //刚体速度 等于 传入二维向量 * 移动速度
        //_rigidbody.velocity = moveInput * moveSpeed;
        
        //将带参数的协程用协程变量存储起来，调用移动协程前先停止上一个移动协程，否则上个协程会先执行下去
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        
        //玩家移动方向为移动输入参数归一化之后的值
        _moveDirection = moveInput.normalized;
        //旋转角度，moveInput的Y值在-1和 1之间
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        //调用移动协程，对moveInput做归一化，使键盘与手柄输入的二维向量保持一致
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, _moveDirection * moveSpeed, moveRotation));
        //先停用玩家减速协程
        StopCoroutine(nameof(DecelerationCoroutine));
        //启用范围移动限制移动协程
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    //停止移动事件
    void StopMove()
    {
        // if (moveCoroutine != null)
        //     StopCoroutine(moveCoroutine);
        // moveDirection=Vector2.zero;
        // moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        // StopCoroutine(nameof(DecelerationCoroutine));
        
        //刚体速度为0
       // _rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        _moveDirection=Vector2.zero;
        //调用移动协程，传入减速时间，刚体速度逐渐为0，机体旋转复原
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,quaternion.identity));
        //停用玩家机体减速协程
        StopCoroutine(nameof(DecelerationCoroutine));
    }
    
    //移动协程，玩家刚体速度从0到最大速度，最大速度到0
    IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)
    {
        //记录时间
        _t = 0f;
        //记录玩家刚体速度值与旋转角度
        _previousVelocity = _rigidbody.velocity;
        _previousRotation = transform.rotation;
        //当t小于指定时间，一直循环
        while (_t < time) 
        {
            //时间累加
            _t += Time.fixedDeltaTime;
            //线性插值改变刚体速度
            _rigidbody.velocity = Vector2.Lerp(_previousVelocity, moveVelocity, _t / time);
            //线性插值改变玩家的旋转角度
            transform.rotation = Quaternion.Lerp(_previousRotation, moveRotation, _t / time);
            //挂起等待下一次固定更新
            yield return  _waitForFixedUpdate;
        }
    }
    /// <summary>
    /// 限制玩家移动范围
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRangeLimitationCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, _paddingX, _paddingY);
            yield return null;
        }
    }
    //玩家机体减速协程
    IEnumerator DecelerationCoroutine()
    {
        //挂起等待减速时间
        yield return waitDecelerationTime;
        //停用移动限位置限制协程
        StopCoroutine(nameof(MoveRangeLimitationCoroutine));
    }

    #endregion

    #region FIRE

    //开火函数
    void Fire()
    {
        //播放枪口特效
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }
    //停火函数
    void StopFire()
    {
        //停播枪口特效
        muzzleVFX.Stop();
        
        StopCoroutine(nameof(FireCoroutine));
    }
    
    //子弹生成协程
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            //未采用对象池生成子弹
            //switch (weaponPower)
            //{
            //    case 0:
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        break;
            //    case 1:
            //        Instantiate(projectile1, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile1, muzzleBottom.position, Quaternion.identity);
            //        break;
            //    case 2:
            //        Instantiate(projectile1, muzzleMiddle.position, Quaternion.identity);
            //        Instantiate(projectile2, muzzleTop.position, Quaternion.identity);
            //        Instantiate(projectile3, muzzleBottom.position, Quaternion.identity);
            //        break;
            //}
            switch (weaponPower) // 根据武器威力生成不同子弹
            {
                //处于能量爆发状态，子弹池生成特殊子弹，否则生成普通子弹
                case 0:
                    PoolManager.Release(_isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(_isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(_isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(_isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(_isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(_isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
            }
            //发射一次子弹，就播放一次随机音效
            AudioManager.Instance.PlayerRandomSFX(projectileSFX);
            //如果处于能量爆发状态，则等待挂起能量爆发开火间隔时间，否则挂起等待普通开火间隔时间
            yield return _isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    
    #endregion

    #region DODGE
    //闪避事件处理函数
    void Dodge()
    {
        //假如正在闪避中且能量不足以闪避时，直接返回
        if (_isDodging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //开启闪避协程
        StartCoroutine(nameof(DodgeCoroutine));
    }
    //闪避协程  
    IEnumerator DodgeCoroutine()
    {
        //闪避开启，正在闪避 为 True
        _isDodging = true;
        //播放闪避音效
        AudioManager.Instance.PlayerRandomSFX(dodgeSFX);
        //消耗能量
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //使碰撞体变为触发器，碰撞函数失效，玩家无敌
        _collider.isTrigger = true;
        //机体开始翻滚前，重置当前滚转角，设置为0
        _currentRoll = 0f;
        //临时变量存储玩家当前缩放值
        var scale = transform.localScale;
        //调用子弹时间函数
        TimeController.Instance.BulletTime(slowMotionDuration,slowMotionDuration);
        //让玩家沿着X轴旋转 改变模型的大小，模型缩放值在1到0.5之间变换

        // * Mathod 01
        
        // while (currentRoll < maxRoll)
        // {
        //     //currentRoll += rollSpeed * Time.deltaTime;
        //     //transform.rotation = Quaternion.AngleAxis(currentRoll,Vector3.right);
        //
        //     if (currentRoll < maxRoll / 2f)
        //     {
        //         scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //         scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //         scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //     }
        //     else
        //     {
        //         scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //         scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //         scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //     }
        //
        //     transform.localScale = scale;
        //     yield return null;
        // }

        // * Mathod 02 使用纯性插值实现模型缩放
        // var t1 = 0f;
        // var t2 = 0f;
        // while (currentRoll < maxRoll)
        // {
        //     currentRoll += rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
        //     if (currentRoll < maxRoll / 2f)
        //     {
        //         t1 += Time.deltaTime / dodgeDuration;
        //         transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);
        //     }
        //     else
        //     {
        //         t2 += Time.deltaTime / dodgeDuration;
        //         transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
        //     }
        //     
        //     yield return null;
        // }
        
        // * Method 03 二阶贝塞尔曲线插值法 

        //当前滚转角 小于 最大滚转角时，一起循环
        while (_currentRoll < maxRoll)
        {
            //当前滚转角累加
            _currentRoll += rollSpeed * Time.deltaTime;
            //机体绕X轴旋转到滚转角位置
            transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
            //模型缩放值在1到0.2之间变换，用二次贝赛尔曲线插值算法。贝赛尔曲线不会穿过最小点，最小缩放值要更小点
            transform.localScale =
                BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, _currentRoll / maxRoll);
            yield return null;
        }
        //触发器恢复为碰撞体，碰撞函数生效，玩家失去无敌效果
        _collider.isTrigger = false;
        //闪避动作完成，是否正在闪避为 false
        _isDodging = false;
    }

    #endregion

    #region OVERDRIVE

    //能量爆发事件处理函数
    void Overdrive()
    {
        //玩家能量不满时直接返回
        if(!PlayerEnergy.Instance.IsEnough(PlayerEnergy.Max)) return;
        //调用能量爆发系统 On委托，开启能量爆发
        PlayerOverdrive.on.Invoke();
    }
    //开启能量爆发函数
    void OverdriveOn()
    {
        //能量爆发处于开启
        _isOverdriving = true;
        //闪避能量消耗提升
        dodgeEnergyCost *= overdriveDodgeFactor;
        //移动速度提升
        moveSpeed *= overdriveSpeedFactor;
        //调用子弹时间函数,时间变慢后再恢复
        TimeController.Instance.BulletTime(slowMotionDuration,slowMotionDuration);
    }
    //关闭能量爆发函数
    void OverdriveOff()
    {
        //能量爆发处于关闭
        _isOverdriving = false;
        //闪避能量消耗恢复
        dodgeEnergyCost /= overdriveDodgeFactor;
        //移动速度恢复
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    #region MISSILE
    //发射导弹事件处理器
    void LaunchMissile()
    {
        //调用导弹系统的发射函数
        _missile.Launch(muzzleMiddle);
    }
    //玩家食取导弹函数
    public void PickUpMissile()
    {
        //调用导弹系统的拾取函数
        _missile.PickUp();
    }
    #endregion

    #region WEAPON POWER

    //武器威力提升函数
    public void PowerUp()
    {
        //武器威力最高为2
        weaponPower = Mathf.Min(weaponPower + 1,2);
    }

    //武器威力下降函数
    void PowerDown()
    {
        //武器威力低为0
        weaponPower = Mathf.Max(--weaponPower, 0);
    }

    #endregion
    
    
    //--------------------------
    // private void Update()
    // {
    //     transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position);
    // }
}
