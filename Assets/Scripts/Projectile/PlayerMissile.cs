using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家导弹
/// </summary>
public class PlayerMissile : PlayerProjectileOverdrive
{
   /// <summary>
   /// 补获目标的语音
   /// </summary>
   [SerializeField] private AudioData targetAcquiredVoice = null;

   [Header("======= SPEED CHANGE ======")]

   /// <summary>
   /// 导弹的慢速度
   /// </summary>
   [SerializeField] private float lowSpeed = 8f;

   /// <summary>
   /// 导弹的高速度
   /// </summary>
   [SerializeField] private float highSpeed = 25f;

   /// <summary>
   /// 导弹变速延迟
   /// </summary>
   [SerializeField] private float variableSpeedDelay = 0.5f;

   [Header("======= EXPLOSION ======")] 
   /// <summary>
   /// 导弹爆炸视效
   /// </summary>
   [SerializeField] private GameObject explosionVFX = null;
   /// <summary>
   /// 导弹爆炸声效
   /// </summary>
   [SerializeField] private AudioData explosionSFX = null;

   /// <summary>
   /// 敌人的碰撞层
   /// </summary>
   [SerializeField] private LayerMask enemyLayerMask = default;

   /// <summary>
   /// 爆炸半径
   /// </summary>
   [SerializeField] private float explosionRadius = 3f;
   /// <summary>
   /// 爆炸伤害
   /// </summary>
   [SerializeField] private float explosionDamage = 100f;

   /// <summary>
   /// 等待变速的时间
   /// </summary>
   private WaitForSeconds waitVariableSpeedDelay;

   protected override void Awake()
   {
      base.Awake();
      waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
   }

   protected override void OnEnable()
   { 
      
      base.OnEnable();
      StartCoroutine(nameof(VariableSpeedCoroutine));
   }
   protected override void OnCollisionEnter2D(Collision2D collision)
   {
      base.OnCollisionEnter2D(collision);

      var position = transform.position;
      PoolManager.Release(explosionVFX, position);
      AudioManager.Instance.PlayerRandomSFX(explosionSFX);

      //获取范围检测内的所有碰撞体，调用敌人的受伤函数
      var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);
      foreach (var collider in colliders)
      {
         if (collider.TryGetComponent<Enemy>(out Enemy enemy))
         {
            enemy.TakeDamage(explosionDamage);
         }
      }
   }
   
   /// <summary>
   /// 在场景中画出爆炸范围
   /// </summary>
   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position,explosionRadius);
   }
   /// <summary>
   /// 变速协程
   /// </summary>
   /// <returns></returns>
   IEnumerator VariableSpeedCoroutine()
   {
      moveSpeed = lowSpeed;
      yield return waitVariableSpeedDelay;
      moveSpeed = highSpeed;
      if (target != null)
         AudioManager.Instance.PlayerRandomSFX(targetAcquiredVoice);
   }
}
