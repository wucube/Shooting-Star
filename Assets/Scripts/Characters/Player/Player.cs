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
    /// 玩家移动加速度
    /// </summary>
    [SerializeField] float accelerationTime = 3f;

    /// <summary>
    /// 玩家移动减速度
    /// </summary>
    [SerializeField] float decelerationTime = 3f;

    /// <summary>
    /// 玩家移动旋转角度
    /// </summary>
    [SerializeField] float moveRotationAngle = 50f;

    //���Ѫ���Ƿ�ȫ������
    public bool IsFullHealth => health == maxHealth;
    //������������Ƿ���������
    public bool IsFullPower => weaponPower == 2;

    [FormerlySerializedAs("projectile")]
    [Header("------ FIRE -------")]
    //�ӵ�����
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    //���������ӵ�Ԥ����
    [SerializeField] private GameObject projectileOverdrive;
    
    //ǹ����Ч
    [SerializeField] private ParticleSystem muzzleVFX;
    
    //�ӵ�����λ��  ǹ��
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    //�ӵ��������Ƶ���ݱ���
    [SerializeField] AudioData projectileSFX;
    //��������
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    
    //������
    [SerializeField] float fireInterval = 0.2f;

    [Header("------ DODGE -------")] 
    //������Ƶ����
    [SerializeField] private AudioData dodgeSFX;
    //������������ֵ
    [SerializeField, Range(0, 100)] private int dodgeEnergyCost = 25;
    //����ת��
    [SerializeField] private float maxRoll = 360f;
    //�����ٶ�
    [SerializeField] private float rollSpeed = 360f;
    //����ʱģ��������Сֵ
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);


    [Header("------ OVERDRIVE -------")]
    //��������ʱ��������������
    [SerializeField] private int overdriveDodgeFactor = 2;
    //��������ʱ���ٶ���������
    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    //��������ʱ�Ŀ�����������
    [SerializeField] private float overdriveFireFactor = 1.2f;
   
    //����޵г���ʱ��
    readonly float InvincibleTime = 1f;
    
    //�Ƿ�����������
    private bool _isDodging = false;
    //����Ƿ�����������
    private bool _isOverdriving = false;
    //�ӵ�ʱ��ָ�����ʱ�䣬ֻ������
    private readonly float slowMotionDuration = 1f;
    
    //�洢��ǰ�Ĺ�ת��
    private float _currentRoll;
    //���ܳ���ʱ��
    private float _dodgeDuration;
    
    //��¼Э��ѭ����ʱ��
    private float _t;
    //����ƶ�����
    private Vector2 _moveDirection;
    //���ڼ�¼��Ҹ����ʼ�ٶ�
    private Vector2 _previousVelocity;
    //���ڼ�¼��ҳ�ʼ��Ԫ��
    private Quaternion _previousRotation;
    
    //Э�̵��ӵ����ɵȴ�����ʱ��
    WaitForSeconds waitForFireInterval;
    //�ȴ���������ʱ�Ŀ�����
    WaitForSeconds waitForOverdriveFireInterval;
    //�ȴ�����ֵ����ʱ��
    WaitForSeconds waitHealthRegenerateTime;
    //�ȴ�����ʱ��
    WaitForSeconds waitDecelerationTime;
    //�ȴ��޵�ʱ��
    private WaitForSeconds waitInvincibleTime;
    //�ȴ���һ�ι̶�֡����
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    //���ڴ洢���������ƶ�Э�̺����ı���
    Coroutine moveCoroutine;
    //����ֵ�ָ�Э�̱���
    Coroutine healthRegenerateCoroutine;

    Rigidbody2D _rigidbody;
    //2D��ײ�����
    private  Collider2D _collider;
     
    //����ϵͳ����
    private MissileSystem _missile;
    
    void Awake()
    {
        //��ȡ�������������
        _rigidbody = GetComponent<Rigidbody2D>();
        //��ȡ�����ײ�����������
        _collider = GetComponent<Collider2D>();
        //ȡ�õ����ű�ϵͳʵ��
        _missile = GetComponent<MissileSystem>();
        
        //ͨ��ģ�Ͷ������Ⱦ�����ȡ��ģ�ͳߴ�
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        //ģ�����ĵ��߾�ֵ Ϊģ�ͳ�����һ��
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
        
        //��ʼ������ʱ��
        _dodgeDuration = maxRoll / rollSpeed;
        
        _rigidbody.gravityScale = 0f;
        //��ʼ���ȴ����������Ŀ���ʱ����������Ϊ ������ʱ�� ���� ������������������ʵ�ʿ�����ʱ������
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval/=overdriveFireFactor);
        
        waitForFireInterval = new WaitForSeconds(fireInterval);
        //��ʼ������ֵ����ʱ��
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        //��ʼ���ȴ�����ʱ�䣬������Ҽ���ʱ��
        waitDecelerationTime = new WaitForSeconds(decelerationTime);
        
        //��ʼ���ȴ��޵�ʱ��
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);
    }
    
    //������д
    protected override void OnEnable()
    {
        base.OnEnable();
        //�����������ƶ���ֹͣ�ƶ����¼����Ķ�Ӧ��������
        input.onMove += Move;
        input.onStopMove += StopMove;
        //���������Ŀ���ͣ���¼����Ķ�Ӧ�¼�������
        input.onFire += Fire;
        input.onStopFire += StopFire;
        //�������������������¼�
        input.onDodge += Dodge;
        //�����������������������¼�
        input.onOverdrive += Overdrive;
        //���ķ��䵼���¼�
        input.onLaunchMissile += LaunchMissile;
        //������������ϵͳ�Ŀ�����ر�ί��
        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }
    void OnDisable()
    {
        //�����������ƶ���ֹͣ�ƶ��¼��˶���Ӧ�¼�������
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        //���������Ŀ���ͣ���¼��˶���Ӧ�¼�������
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        //�˶����������������¼�
        input.onDodge -= Dodge;
        //�˶����������¼�
        input.onOverdrive -= Overdrive;
        //�˶����䵼���¼�
        input.onLaunchMissile -= LaunchMissile;
        //�˶���������ϵͳ�Ŀ����ر�ί��
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�޸ĸ������������ֵΪ0����������Ϊ0�� ��Ϸ����ʱ���Ǹ�׹�� 
        _rigidbody.gravityScale = 0f;
        
        //��ʼ���ӵ����ɵȴ�����ʱ��
        waitForFireInterval = new WaitForSeconds(fireInterval);
        //��ʼ����Ѫ�ȴ�����ʱ��
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        
        //��ʼ��HUDѪ��
        statsBar_HUD.Initialize(health, maxHealth);
        
        //Player�ű�����ʱ������Gameplay������
        input.EnableGameplayInput();
    }

    //��д���˺���
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //�������ʱ���������½�
        PowerDown();
        
        //����HUDѪ��״̬
        statsBar_HUD.UpdateStats(health, maxHealth);
        //�����ӵ�ʱ�亯��
        TimeController.Instance.BulletTime(slowMotionDuration);
        
        //�����Ϸ�����ڻ״̬
        if (gameObject.activeSelf)
        {
            //����ʱ����Move����
            Move(_moveDirection);
            //��������޵�ʱ��Э��
            StartCoroutine(InvincibleCoroutine());
            
            //�����������ؿ���
            if (regenerateHealth)
            {
                //�������Э�̲�Ϊ�գ���ͣ������Э�̣�ȷ��ÿ�ε���Э��ʱ����ǰ�Ļ�ѪЭ���Ѿ�ͣ��
                if (healthRegenerateCoroutine != null)
                    StopCoroutine(healthRegenerateCoroutine);
                //������������Э�̣���������ʱ���������ָ��ٷֱ���
                healthRegenerateCoroutine =
                    StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }
    //��д��Ѫ����
    public override void RestoreHealth(float value)
    {
        //���û��ຯ��
        base.RestoreHealth(value);
        //����Ѫ��״̬
        statsBar_HUD.UpdateStats(health, maxHealth);
    }
    //��д��������
    public override void Die()
    {
        //������Ϸ����������Ϸ����ί��
        GameManager.onGameOver?.Invoke();
        //��Ϸ״̬Ϊ����
        GameManager.GameState = GameState.GameOver;
        
        //�ȸ���Ѫ��״̬����ǰ״ֵ̬Ϊ0��Ѫ�����
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
    }

    //�޵�ʱ��Э��
    IEnumerator InvincibleCoroutine()
    {
        //�����ײ�崥��������һ��ʱ����ٹر�
        _collider.isTrigger = true;
        yield return waitInvincibleTime;
        _collider.isTrigger = false;
    }
    #region MOVE

    //�ƶ��¼���������
    void Move(Vector2 moveInput)
    {
        //�����ٶ� ���� �����ά���� * �ƶ��ٶ�
        //_rigidbody.velocity = moveInput * moveSpeed;
        
        //����������Э����Э�̱����洢�����������ƶ�Э��ǰ��ֹͣ��һ���ƶ�Э�̣������ϸ�Э�̻���ִ����ȥ
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        
        //����ƶ�����Ϊ�ƶ����������һ��֮���ֵ
        _moveDirection = moveInput.normalized;
        //��ת�Ƕȣ�moveInput��Yֵ��-1�� 1֮��
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        //�����ƶ�Э�̣���moveInput����һ����ʹ�������ֱ�����Ķ�ά��������һ��
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, _moveDirection * moveSpeed, moveRotation));
        //��ͣ����Ҽ���Э��
        StopCoroutine(nameof(DecelerationCoroutine));
        //���÷�Χ�ƶ������ƶ�Э��
        StartCoroutine(nameof(MoveRangeLimitationCoroutine));
    }
    //ֹͣ�ƶ��¼�
    void StopMove()
    {
        // if (moveCoroutine != null)
        //     StopCoroutine(moveCoroutine);
        // moveDirection=Vector2.zero;
        // moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        // StopCoroutine(nameof(DecelerationCoroutine));
        
        //�����ٶ�Ϊ0
       // _rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        _moveDirection=Vector2.zero;
        //�����ƶ�Э�̣��������ʱ�䣬�����ٶ���Ϊ0��������ת��ԭ
        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,quaternion.identity));
        //ͣ����һ������Э��
        StopCoroutine(nameof(DecelerationCoroutine));
    }
    
    //�ƶ�Э�̣���Ҹ����ٶȴ�0������ٶȣ�����ٶȵ�0
    IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)
    {
        //��¼ʱ��
        _t = 0f;
        //��¼��Ҹ����ٶ�ֵ����ת�Ƕ�
        _previousVelocity = _rigidbody.velocity;
        _previousRotation = transform.rotation;
        //��tС��ָ��ʱ�䣬һֱѭ��
        while (_t < time) 
        {
            //ʱ���ۼ�
            _t += Time.fixedDeltaTime;
            //���Բ�ֵ�ı�����ٶ�
            _rigidbody.velocity = Vector2.Lerp(_previousVelocity, moveVelocity, _t / time);
            //���Բ�ֵ�ı���ҵ���ת�Ƕ�
            transform.rotation = Quaternion.Lerp(_previousRotation, moveRotation, _t / time);
            //����ȴ���һ�ι̶�����
            yield return  _waitForFixedUpdate;
        }
    }
    /// <summary>
    /// ��������ƶ���Χ
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
    //��һ������Э��
    IEnumerator DecelerationCoroutine()
    {
        //����ȴ�����ʱ��
        yield return waitDecelerationTime;
        //ͣ���ƶ���λ������Э��
        StopCoroutine(nameof(MoveRangeLimitationCoroutine));
    }

    #endregion

    #region FIRE

    //������
    void Fire()
    {
        //����ǹ����Ч
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }
    //ͣ����
    void StopFire()
    {
        //ͣ��ǹ����Ч
        muzzleVFX.Stop();
        
        StopCoroutine(nameof(FireCoroutine));
    }
    
    //�ӵ�����Э��
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            //δ���ö���������ӵ�
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
            switch (weaponPower) // ���������������ɲ�ͬ�ӵ�
            {
                //������������״̬���ӵ������������ӵ�������������ͨ�ӵ�
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
            //����һ���ӵ����Ͳ���һ�������Ч
            AudioManager.Instance.PlayerRandomSFX(projectileSFX);
            //���������������״̬����ȴ�������������������ʱ�䣬�������ȴ���ͨ������ʱ��
            yield return _isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
        }
    }
    
    #endregion

    #region DODGE
    //�����¼���������
    void Dodge()
    {
        //������������������������������ʱ��ֱ�ӷ���
        if (_isDodging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        //��������Э��
        StartCoroutine(nameof(DodgeCoroutine));
    }
    //����Э��  
    IEnumerator DodgeCoroutine()
    {
        //���ܿ������������� Ϊ True
        _isDodging = true;
        //����������Ч
        AudioManager.Instance.PlayerRandomSFX(dodgeSFX);
        //��������
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //ʹ��ײ���Ϊ����������ײ����ʧЧ������޵�
        _collider.isTrigger = true;
        //���忪ʼ����ǰ�����õ�ǰ��ת�ǣ�����Ϊ0
        _currentRoll = 0f;
        //��ʱ�����洢��ҵ�ǰ����ֵ
        var scale = transform.localScale;
        //�����ӵ�ʱ�亯��
        TimeController.Instance.BulletTime(slowMotionDuration,slowMotionDuration);
        //���������X����ת �ı�ģ�͵Ĵ�С��ģ������ֵ��1��0.5֮��任

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

        // * Mathod 02 ʹ�ô��Բ�ֵʵ��ģ������
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
        
        // * Method 03 ���ױ��������߲�ֵ�� 

        //��ǰ��ת�� С�� ����ת��ʱ��һ��ѭ��
        while (_currentRoll < maxRoll)
        {
            //��ǰ��ת���ۼ�
            _currentRoll += rollSpeed * Time.deltaTime;
            //������X����ת����ת��λ��
            transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
            //ģ������ֵ��1��0.2֮��任���ö��α��������߲�ֵ�㷨�����������߲��ᴩ����С�㣬��С����ֵҪ��С��
            transform.localScale =
                BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, _currentRoll / maxRoll);
            yield return null;
        }
        //�������ָ�Ϊ��ײ�壬��ײ������Ч�����ʧȥ�޵�Ч��
        _collider.isTrigger = false;
        //���ܶ�����ɣ��Ƿ���������Ϊ false
        _isDodging = false;
    }

    #endregion

    #region OVERDRIVE

    //���������¼���������
    void Overdrive()
    {
        //�����������ʱֱ�ӷ���
        if(!PlayerEnergy.Instance.IsEnough(PlayerEnergy.Max)) return;
        //������������ϵͳ Onί�У�������������
        PlayerOverdrive.on.Invoke();
    }
    //����������������
    void OverdriveOn()
    {
        //�����������ڿ���
        _isOverdriving = true;
        //����������������
        dodgeEnergyCost *= overdriveDodgeFactor;
        //�ƶ��ٶ�����
        moveSpeed *= overdriveSpeedFactor;
        //�����ӵ�ʱ�亯��,ʱ��������ٻָ�
        TimeController.Instance.BulletTime(slowMotionDuration,slowMotionDuration);
    }
    //�ر�������������
    void OverdriveOff()
    {
        //�����������ڹر�
        _isOverdriving = false;
        //�����������Ļָ�
        dodgeEnergyCost /= overdriveDodgeFactor;
        //�ƶ��ٶȻָ�
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    #region MISSILE
    //���䵼���¼�������
    void LaunchMissile()
    {
        //���õ���ϵͳ�ķ��亯��
        _missile.Launch(muzzleMiddle);
    }
    //���ʳȡ��������
    public void PickUpMissile()
    {
        //���õ���ϵͳ��ʰȡ����
        _missile.PickUp();
    }
    #endregion

    #region WEAPON POWER

    //����������������
    public void PowerUp()
    {
        //�����������Ϊ2
        weaponPower = Mathf.Min(weaponPower + 1,2);
    }

    //���������½�����
    void PowerDown()
    {
        //����������Ϊ0
        weaponPower = Mathf.Max(--weaponPower, 0);
    }

    #endregion
    
    
    //--------------------------
    // private void Update()
    // {
    //     transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position);
    // }
}
