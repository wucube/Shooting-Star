using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    //�����������ʱ��
    [SerializeField] private float continuousFireDuration = 1.5f;

    [Header("======= Player Detection ========")] 
    //���еı任�������
    [SerializeField] private Transform playerDetectionTransform;
    //���еĳߴ�
    [SerializeField] private Vector3 playerDetectionSize;
    //��Ҳ����ֱ���
    [SerializeField] private LayerMask playerLayer;

    [Header("======= Beam ========")] 
    //������ȴʱ��
    [SerializeField] private float beamCooldownTime = 12f;
    //��������������Ч
    [SerializeField] private AudioData beamChargingSFX;
    //���ⷢ����Ч
    [SerializeField] private AudioData beamLaunchSFX;
    //���������Ƿ���ȴ���
    private bool _isBeamReady;
    //�����Trigger����ת��Ϊ��ϣֵʹ��
    private int launchBeamID = Animator.StringToHash("launchBeam");
    //��С������ʱ��
    private WaitForSeconds waitForContinuousFireInterval;
    
    private WaitForSeconds waitForFireInterval;
    //�ȴ�������ȴʱ��
    private WaitForSeconds _waitBeamCooldownTime;

    //�ӵ������б� ��ϻ
    private List<GameObject> magazine;
    //��ſ�����Ч
    private AudioData launchSFX;
    //�������
    private Animator _animator;
    //���λ��
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        //ȡ��bossԤ�����ϵĶ��������
        _animator = GetComponent<Animator>();
        
        //��ʼ����С������ʱ��
        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        //��ʼ���������ʱ��
        waitForFireInterval = new WaitForSeconds(maxFireInterval);
        //�ȴ�������ȴʱ���ʼ��
        _waitBeamCooldownTime = new WaitForSeconds(beamCooldownTime);
        //��ʼ����ϻ�б������ӵ����鳤�ȿ����б�����
        magazine = new List<GameObject>(projectiles.Length);
        //��ʼ�����λ����Ϣ��ȡ����ҵı任���
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //��дOnEnable����
    protected override void OnEnable()
    {
        //����û׼����
        _isBeamReady = false;
        //ֹͣ����ǹ����Ч
        muzzleVFX.Stop();
        //���ü�����ȴЭ��
        StartCoroutine(nameof(BeamCooldownCoroutine));
        //���û��ຯ��
        base.OnEnable();
    }

    //����Boss���еķ�Χ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }

    //���������������뷢��
    void ActivateBeamWeapon()
    {
        //��������û��׼����
        _isBeamReady = false;
        //֪ͨ���������ŷ��伤��Ķ���
        _animator.SetTrigger(launchBeamID);
        //���ż�������������Ч
        AudioManager.Instance.PlayerRandomSFX(beamChargingSFX);
    }

    //�����¼����������伤��ʱ���ŷ�����Ч
    void AnimationEventLaunchBeam()
    {
        AudioManager.Instance.PlayerRandomSFX(beamLaunchSFX);
    }

    //�����¼�������ֹͣ���伤��
    void AnimationEventStopBeam()
    {
        //ֹͣ׷�����Э��
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        //�ٴο���������������ȴʱ��
        StartCoroutine(nameof(BeamCooldownCoroutine));
        //�ٴο������رյ��������Э��
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }
    //װ���ӵ�
    void LoadProjectiles()
    {
        //������ӵ��б�
        magazine.Clear();
        //��һ�����Boss��ǰ��ʱ
        if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))
        {
            //װ��1���ӵ�
            magazine.Add(projectiles[0]);
            //�����ӵ�������Ч
            launchSFX = projectileLaunchSFX[0];
        }
        else //��һ��岻��Boss��ǰ��
        {
            if (Random.value < 0.5f)
            {
                //װ��2���ӵ�
                magazine.Add(projectiles[1]);
                launchSFX = projectileLaunchSFX[1];
            }
            else
            {
                //װ�����в�ͬ�Ƕȵ�3���ӵ�
                for (int i = 2; i < projectiles.Length; i++)
                    magazine.Add(projectiles[i]);
                
                launchSFX = projectileLaunchSFX[2];
            }
        }
    }

    //��д�������Э��
    protected override IEnumerator RandomlyFireCoroutine()
    {
        //boss���ڻ״̬����һֱѭ��
        while (isActiveAndEnabled)
        {
            //��Ϸ����ʱ��ֹͣЭ��
            if (GameManager.GameState == GameState.GameOver) yield break;
            //���������������ȴ���
            if (_isBeamReady)
            {
                //���������
                ActivateBeamWeapon();
                //����׷�����Э��
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                //ֹͣ����Э��
                yield break;
            }
            //����ȴ�һ��ʱ��
            yield return waitForFireInterval;
            //����ִ����������Э��
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    //��������Э��
    IEnumerator ContinuousFireCoroutine()
    {
        //����װ���ӵ�����
        LoadProjectiles();
        //����ǹ����Ч
        muzzleVFX.Play();
        
        //���������ʱ��
        float continuousFireTimer = 0f;
        //���������ʱ�� С�� �����������ʱ�䣬��һֱ����
        while (continuousFireTimer < continuousFireDuration)
        {
            //���䵯ϻ�е��ӵ�
            foreach (var projectile in magazine)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            //��ʱ�����ӿ�����ʱ��ֵ
            continuousFireTimer += minFireInterval;
            //���ſ�����Ч
            AudioManager.Instance.PlayerRandomSFX(launchSFX);
            //����ȴ���С������ʱ��
            yield return waitForContinuousFireInterval;
        }
        //ֹͣ����ǹ����Ч
        muzzleVFX.Stop();
    }

    //����������ȴЭ��
    IEnumerator BeamCooldownCoroutine()
    {
        //�ȴ����𼤹���ȴʱ��
        yield return _waitBeamCooldownTime;
        //������׼�����
        _isBeamReady = true;
    }

    //׷�����Э��
    IEnumerator ChasingPlayerCoroutine()
    {
        //Boss����ʱһֱѭ��
        while (isActiveAndEnabled)
        {
            //�޸�Boss�ƶ�ʱ��Ŀ��λ��
            //X��Ϊ�������ұ� ��ȥ Bossģ�͵�x�߾�ֵ
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            //Y��Ϊ���λ�õ�Y��
            targetPosition.y = playerTransform.position.y;
            yield return null;
        }
    }
}
