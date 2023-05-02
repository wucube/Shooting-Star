using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
   //��Ч����
   [SerializeField] private AudioData targetAcquiredVoice = null;
   [Header("======= SPEED CHANGE ======")]
   //�����ƶ�ʱ�����ٶ�
   [SerializeField] private float lowSpeed = 8f;
   //�����ƶ�ʱ���ٶ�
   [SerializeField] private float highSpeed = 25f;
   //���ٵ��ӳ�ʱ��
   [SerializeField] private float variableSpeedDelay = 0.5f;

   [Header("======= EXPLOSION ======")] 
   //��ը�Ӿ���Ч����
   [SerializeField] private GameObject explosionVFX = null;
   //��ը��Ч����
   [SerializeField] private AudioData explosionSFX = null;
   //���˲㼶����
   [SerializeField] private LayerMask enemyLayerMask = default;
   //������ը(�˺�)��Χ�뾶
   [SerializeField] private float explosionRadius = 3f;
   //��ը�˺�ֵ
   [SerializeField] private float explosionDamage = 100f;
   //�ȴ������ӳ�ʱ��
   private WaitForSeconds _waitVariableSpeedDelay;

   protected override void Awake()
   {
      base.Awake();
      //��ʼ���ȴ������ӳ�ʱ��
      _waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
   }

   protected override void OnEnable()
   { 
      //���ຯ���������Ƶ�ϵͳ
      base.OnEnable();
      //��������Э��
      StartCoroutine(nameof(VariableSpeedCoroutine));
   }
   //��д��ײ����
   protected override void OnCollisionEnter2D(Collision2D collision)
   {
      base.OnCollisionEnter2D(collision);
      //��������ɱ�ը��Ч
      var position = transform.position;
      PoolManager.Release(explosionVFX, position);
      //���ű�ը��Ч
      AudioManager.Instance.PlayerRandomSFX(explosionSFX);
      //�ڵ�����ը�ڵ����е�����ײ��
      var colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius,enemyLayerMask);
      foreach (var collider in colliders)
      {
         //ȡ�õ��˽ű�
         if (collider.TryGetComponent<Enemy>(out Enemy enemy))
         {
            //���õ������˺���
            enemy.TakeDamage(explosionDamage);
         }
      }
   }
   
   //���Ƽ򵥼���ģ��
   private void OnDrawGizmosSelected()
   {
      //����������ɫ
      Gizmos.color = Color.yellow;
      //����һ���߿�����
      Gizmos.DrawWireSphere(transform.position,explosionRadius);
   }
   //����Э��
   IEnumerator VariableSpeedCoroutine()
   {
      //�ƶ��ٶ��ȵ���
      moveSpeed = lowSpeed;
      //����ȴ�һ��ʱ��
      yield return _waitVariableSpeedDelay;
      //�ƶ��ٶ���Ϊ����
      moveSpeed = highSpeed;
      //���Ŀ�겻Ϊ�գ�����һ���ض���Ч
      if (target != null)
         AudioManager.Instance.PlayerRandomSFX(targetAcquiredVoice);
   }
}
