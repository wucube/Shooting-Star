using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

//运行时为对象添加2D刚体
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    /// <summary>
    /// 玩家HUD状态条
    /// </summary>
    [SerializeField] StatsBar_HUD statsBar_HUD;

    /// <summary>
    /// 是否恢复血量值
    /// </summary>
    [SerializeField] bool regenerateHealth = true;

    /// <summary>
    /// 血量恢复的时间
    /// </summary>
    [SerializeField] float healthRegenerateTime;
    
    /// <summary>
    /// 血量恢复百分比
    /// </summary>
    /// <returns></returns>
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    /// <summary>
    /// 玩家输入类
    /// </summary>
    [Header("------ INPUT -------")] 
    [SerializeField] PlayerInput input;

    /// <summary>
    /// 玩家移动速度
    /// </summary>
    [Header("------ MOVE -------")]
    [SerializeField] float moveSpeed = 10f;
    
    /// <summary>
    /// 玩家机体水平边距
    /// </summary>
    private float paddingX;

    /// <summary>
    /// 玩家机体垂直边距
    /// </summary>
    private float paddingY;
    
    /// <summary>
    /// 玩家加速时间
    /// </summary>
    [SerializeField] float accelerationTime = 3f;

    /// <summary>
    /// 玩家减速时间
    /// </summary>
    [SerializeField] float decelerationTime = 3f;

    /// <summary>
    /// 玩家移动旋转角度
    /// </summary>
    [SerializeField] float moveRotationAngle = 50f;

    /// <summary>
    /// 血量是否全满
    /// </summary>
    public bool IsFullHealth => health == maxHealth;

    /// <summary>
    /// 武器威力全满
    /// </summary>
    public bool IsFullPower => weaponPower == 2;

    [FormerlySerializedAs("projectile")]
    [Header("------ FIRE -------")]
    [SerializeField] private GameObject projectile1;
    [SerializeField] private GameObject projectile2;
    [SerializeField] private GameObject projectile3;
    /// <summary>
    /// 能量爆发子弹
    /// </summary>
    [SerializeField] private GameObject projectileOverdrive;
    
    /// <summary>
    /// 枪口特效
    /// </summary>
    [SerializeField] private ParticleSystem muzzleVFX;
    
    /// <summary>
    /// 中枪口
    /// </summary>
    [SerializeField] Transform muzzleMiddle;

    /// <summary>
    /// 上枪口
    /// </summary>
    [SerializeField] Transform muzzleTop;

    /// <summary>
    /// 下枪口
    /// </summary>
    [SerializeField] Transform muzzleBottom;

    [SerializeField] AudioData projectileSFX;

    /// <summary>
    /// 武器威力
    /// </summary>
    /// <returns></returns>
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    
    /// <summary>
    /// 开火间隔
    /// </summary>
    [SerializeField] float fireInterval = 0.2f;

    [Header("------ DODGE -------")] 

    /// <summary>
    /// 闪避声效
    /// </summary>
    [SerializeField] private AudioData dodgeSFX;
    /// <summary>
    /// 闪避能量消耗值
    /// </summary>
    /// <returns></returns>
    [SerializeField, Range(0, 100)] private int dodgeEnergyCost = 25;
    /// <summary>
    /// 最大翻滚角度
    /// </summary>
    [SerializeField] private float maxRoll = 360f;
    /// <summary>
    /// 翻滚速度
    /// </summary>
    [SerializeField] private float rollSpeed = 360f;

    /// <summary>
    /// 闪避中机体最小绽放值
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);


    [Header("------ OVERDRIVE -------")]
    /// <summary>
    /// 能量爆发闪避因子
    /// </summary>
    [SerializeField] private int overdriveDodgeFactor = 2;
    /// <summary>
    /// 能量爆发速度因子
    /// </summary>
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    /// <summary>
    /// 能量爆发开火因子
    /// </summary>
    [SerializeField] private float overdriveFireFactor = 1.2f;
   
    /// <summary>
    /// 无敌时间
    /// </summary>
    readonly float InvincibleTime = 1f;
    
    /// <summary>
    /// 是否正在闪避
    /// </summary>
    private bool isDodging = false;

    /// <summary>
    /// 是否为能量爆发
    /// </summary>
    private bool isOverdriving = false;
    
    /// <summary>
    /// 慢动作持续时间
    /// </summary>
    private readonly float slowMotionDuration = 1f;
    
    /// <summary>
    /// 当前翻滚角度
    /// </summary>
    private float currentRoll;
    /// <summary>
    /// 闪避持续时间，在机体翻滚完成后结束
    /// </summary>
    private float dodgeDuration;
    
    /// <summary>
    /// 用于移动协程运算的时间
    /// </summary>
    private float t;
    /// <summary>
    /// 移动方向
    /// </summary>
    private Vector2 moveDirection;
    /// <summary>
    /// 先前的速度
    /// </summary>
    private Vector2 previousVelocity;

    /// <summary>
    /// 先前的旋转
    /// </summary>
    private Quaternion previousRotation;
    
    /// <summary>
    /// 等待开火间隔
    /// </summary>
    WaitForSeconds waitForFireInterval;
    /// <summary>
    /// 等待能量爆发开火间隔
    /// </summary>
    WaitForSeconds waitForOverdriveFireInterval;
    /// <summary>
    /// 等待血量恢复间隔
    /// </summary>
    WaitForSeconds waitHealthRegenerateTime;
    /// <summary>
    /// 等待减速时间
    /// </summary>
    WaitForSeconds waitDecelerationTime;
    /// <summary>
    /// 无敌时间间隔
    /// </summary>
    private WaitForSeconds waitInvincibleTime;

    /// <summary>
    /// 等待固定更新
    /// </summary>
    /// <returns></returns>
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    /// <summary>
    /// 移动协程
    /// </summary>
    Coroutine moveCoroutine;

    /// <summary>
    /// 血量恢复协程
    /// </summary>
    Coroutine healthRegenerateCoroutine;

    new Rigidbody2D rigidbody;

    new  Collider2D collider;
     
    /// <summary>
    /// 导弹系统
    /// </summary>
    private MissileSystem missile;
    
    void Awake()
    {
        
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();
        
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
        
        dodgeDuration = maxRoll / rollSpeed;
        //机体刚体重力值为0，模拟太空无重力状态
        rigidbody.gravityScale = 0f;
        
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }
    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    void Start()
    {
        
        //rigidbody.gravityScale = 0f;
        
        //waitForFireInterval = new WaitForSeconds(fireInterval);
        
        //waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        
        statsBar_HUD.Initialize(health, maxHealth);
        
        input.EnableGameplayInput();
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        PowerDown();

        statsBar_HUD.UpdateStats(health, maxHealth);

        TimeController.Instance.BulletTime(slowMotionDuration);
        
        if (gameObject.activeSelf)
        {
            Move(moveDirection);
           
            StartCoroutine(InvincibleCoroutine());
            
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                    StopCoroutine(healthRegenerateCoroutine);
                
                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);

        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        
        GameManager.GameState = GameState.GameOver;
        
        statsBar_HUD.UpdateStats(0f, maxHealth);

        base.Die();
    }
    /// <summary>
    /// 无敌状态协程
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibleCoroutine()
    {
        collider.isTrigger = true;

        yield return waitInvincibleTime;

        collider.isTrigger = false;
    }

    #region MOVE
    /// <summary>
    /// 移动输入事件处理器
    /// </summary>
    /// <param name="moveInput"></param>
    void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null) 
            StopCoroutine(moveCoroutine);

        moveDirection = moveInput.normalized;
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
       
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, moveRotation));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    
    /// <summary>
    /// 停止移动输入事件处理器
    /// </summary>
    void StopMove()
    {
        // if (moveCoroutine != null)
        //     StopCoroutine(moveCoroutine);
        // moveDirection=Vector2.zero;
        // moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        // StopCoroutine(nameof(DecelerationCoroutine));
    
       // _rigidbody.velocity = Vector2.zero;

        if (moveCoroutine != null) 
            StopCoroutine(moveCoroutine);

        moveDirection = Vector2.zero;

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, quaternion.identity));
        StopCoroutine(nameof(DecelerationCoroutine));
    }
    
    /// <summary>
    /// 移动协程
    /// </summary>
    /// <param name="time">移动时间</param>
    /// <param name="moveVelocity">移动速度</param>
    /// <param name="moveRotation">移动旋转角度</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;

        while (t < time) 
        {
            t += Time.fixedDeltaTime;
            
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t / time);
 
            yield return  waitForFixedUpdate;
        }
    }

    /// <summary>
    /// 移动范围限制协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRangeLimitationCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }
    /// <summary>
    /// 减速协程
    /// </summary>
    /// <returns></returns>
    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;

        StopCoroutine(nameof(MoveRangeLimitationCoroutine));
    }

    #endregion

    #region FIRE

    /// <summary>
    /// 开火输入事件处理器
    /// </summary>
    void Fire()
    {
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }
    
    /// <summary>
    /// 停止开火输入事件处理器
    /// </summary>
    void StopFire()
    {
        muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }
    
    /// <summary>
    /// 开火协程
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            //不使用对象池根据武器威力等级实例化子弹对象
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

            switch (weaponPower)
            {
                //使用对象池释放不同武器威力下的不同数量与类型的子弹对象
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                    break;
            }
            AudioManager.Instance.PlayerRandomSFX(projectileSFX);
           
            //根据是否为能量爆发状态决定不同的开火间隔时间
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    #endregion

    #region DODGE
    /// <summary>
    /// 闪避输入事件处理器
    /// </summary>
    void Dodge()
    {
        
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) 
            return;

        StartCoroutine(nameof(DodgeCoroutine));
    }

    /// <summary>
    /// 闪避协程
    /// </summary>
    /// <returns></returns>
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
       
        AudioManager.Instance.PlayerRandomSFX(dodgeSFX);
        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        collider.isTrigger = true;
        
        currentRoll = 0f;

        var scale = transform.localScale;

        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);


        //闪避过程中缩放玩家机体，前半段缩小后半段放大

        // * Mathod 01 缩放向量各方向的值逐个改变
        // while (currentRoll < maxRoll)
        // {
        //     currentRoll += rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(currentRoll,Vector3.right);
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

        // * Mathod 02 使用插值计算改变缩放值
        //
        // var t1 = 0f;
        // var t2 = 0f;
        // while (currentRoll < maxRoll)
        // {
        //     currentRoll += rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
        //
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
        
        // * Method 03 使用贝塞尔曲线变换缩放值
        while (currentRoll < maxRoll)
        {
            //最大翻滚角度与翻滚速度契合，可用翻滚速度累加计算角度
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return null;
        }
        //翻滚完成，取消触发器，玩家机体变得可碰撞
        collider.isTrigger = false;
        isDodging = false;
    }

    #endregion

    #region OVERDRIVE

    /// <summary>
    /// 能量爆发启用输入事件处理器
    /// </summary>
    void Overdrive()
    {
        if(!PlayerEnergy.Instance.IsEnough(PlayerEnergy.Max)) 
            return;
        
        PlayerOverdrive.on.Invoke();
    }
    
    /// <summary>
    /// 进入能量爆发事件处理器
    /// </summary>
    void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration,slowMotionDuration);
    }
    
    /// <summary>
    /// 退出能量爆发事件处理器
    /// </summary>
    void OverdriveOff()
    {
        isOverdriving = false;

        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    #region MISSILE
    /// <summary>
    /// 发射导弹输入的事件处理器
    /// </summary>
    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
    
    /// <summary>
    /// 拾取导弹
    /// </summary>
    public void PickUpMissile()
    {
        missile.PickUp();
    }
    #endregion

    #region WEAPON POWER
    /// <summary>
    /// 武器威力提升
    /// </summary>
    public void PowerUp()
    {
        weaponPower = Mathf.Min(weaponPower + 1, 2);
    }

    //武器威力下降
    void PowerDown()
    {
        weaponPower = Mathf.Max(--weaponPower, 0);
    }

    #endregion
    
    // 在Update中限制玩家移动范围
    // private void Update()
    // {
    //     transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position);
    // }
}
