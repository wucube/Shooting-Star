using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    //�л����ĵ㵽��ԵX��Y���ƫ��ֵ 
    protected float paddingX;
     float _paddingY;
    //�����ƶ��ٶ�
    [SerializeField] float moveSpeed = 2f;
    //�����ƶ���ת����
    [SerializeField] float moveRotationAngle = 25f;

    [Header("---- FIRE ----")]
    //�����ӵ������Ӿ���Ч���󼯺�
    [SerializeField] protected GameObject[] projectiles;
    //���˷����ӵ�����Ƶ���ݼ���
    [SerializeField] protected AudioData[] projectileLaunchSFX;
    
    //����λ�� ǹ��
    [SerializeField] protected Transform muzzle;
    
    //ǹ����Ч����
    [SerializeField] protected ParticleSystem muzzleVFX;
    
    //��С������ʱ��
    [SerializeField] protected float minFireInterval;
    //��󿪻���ʱ��
    [SerializeField] protected float maxFireInterval;
    
    //����Ŀ���ƶ�λ��
    protected Vector3 targetPosition;
    
    //ÿ֡����ƶ�����
    //private float _maxMoveDistancePerFrame;
    //����ȴ���һ���̶�֡
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        //��ʼ��ÿ֡����ƶ�����
        //_maxMoveDistancePerFrame = moveSpeed * Time.fixedDeltaTime;
        //ͨ��ģ�Ͷ������Ⱦ������õ�ģ�͵ĳߴ�ֵ
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        _paddingY = size.y / 2f;
    }
    
    //�л�����һ�ξ͵���һ��
    protected virtual void OnEnable()
    {
        //��������ƶ�Э��
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        //�������
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }
    //��Ϸͣ��ʱ����
    protected virtual void OnDisable()
    {
        //ͣ������Э��
        StopAllCoroutines();
    }
    //��������ƶ�Э��
    protected virtual IEnumerator RandomlyMovingCoroutine() 
    {
        //�л������Ҿ�ͷ������λ��
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, _paddingY);
        //�л���Ŀ��λ�ã��ھ�ͷ�ڵ��Ҳ����λ��
        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, _paddingY);
        //����л�û�б��ݻ���һֱѭ��
        while (gameObject.activeSelf)
        {
            //����л�δ����Ŀ��λ�� ���л���ǰλ����Ŀ��λ�þ��� ���� �л�ÿ֡�ƶ��ľ���
            if (Vector3.Distance(transform.position, targetPosition) >=moveSpeed * Time.fixedDeltaTime)
            {
                //����ǰ��Ŀ��λ�� ��ÿ֡�ƶ����� ǰ��Ŀ��λ��
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                //�����ƶ�ʱ��ת ȡ���ƶ�������������һ����Y��ֵ �� ��ת�Ƕȣ��ص��˶����X����������ת
                transform.rotation =
                    Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle,
                        Vector3.right);
            }
            //�л�����ѵ���Ŀ��λ�ã������һ���µ�Ŀ��λ��
            else 
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX,_paddingY);
            //����ֱ����һ���̶�֡
            yield return new WaitForFixedUpdate();
        }
    }
    
    //�����������Э��
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        //����л�û�дݻ���һ��ѭ��
        while(gameObject.activeSelf)
        {
            //�������С�������У��������ֻ����ѭ�����½�������������ȫʵ��ÿ�����ʱ�俪��
            yield return new WaitForSeconds(Random.Range(minFireInterval,maxFireInterval));
            
            //�����Ϸ״̬Ϊ�����������Э��
            if(GameManager.GameState ==GameState.GameOver) yield break;
            
            foreach (var projectile in projectiles)
            {
                //��������������е������ӵ�
                PoolManager.Release(projectile, muzzle.position);
            }
            //��������ӵ�������Ч
            AudioManager.Instance.PlayerRandomSFX(projectileLaunchSFX);
            //����ǹ����Ч
            muzzleVFX.Play();
        }
    }
}
