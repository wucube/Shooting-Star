using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家导弹
/// </summary>
public class PlayerMissile : PlayerProjectileOverdrive
{
   //音效变量
   [SerializeField] private AudioData targetAcquiredVoice = null;
   [Header("======= SPEED CHANGE ======")]
   //慢速移动时的速速度
   [SerializeField] private float lowSpeed = 8f;
   //高速移动时的速度
   [SerializeField] private float highSpeed = 25f;
   //变速的延迟时间
   [SerializeField] private float variableSpeedDelay = 0.5f;

   [Header("======= EXPLOSION ======")] 
   //爆炸视觉特效对象
   [SerializeField] private GameObject explosionVFX = null;
   //爆炸音效对象
   [SerializeField] private AudioData explosionSFX = null;
   //敌人层级遮罩
   [SerializeField] private LayerMask enemyLayerMask = default;
   //导弹爆炸(伤害)范围半径
   [SerializeField] private float explosionRadius = 3f;
   //爆炸伤害值
   [SerializeField] private float explosionDamage = 100f;
   //等待变速延迟时间
   private WaitForSeconds _waitVariableSpeedDelay;

   protected override void Awake()
   {
      base.Awake();
      //初始化等待变速延迟时间
      _waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
   }

   protected override void OnEnable()
   { 
      //基类函数，开启制导系统
      base.OnEnable();
      //开启变速协程
      StartCoroutine(nameof(VariableSpeedCoroutine));
   }
   //重写碰撞函数
   protected override void OnCollisionEnter2D(Collision2D collision)
   {
      base.OnCollisionEnter2D(collision);
      //对象池生成爆炸特效
      var position = transform.position;
      PoolManager.Release(explosionVFX, position);
      //播放爆炸音效
      AudioManager.Instance.PlayerRandomSFX(explosionSFX);
      //在导弹爆炸内的所有敌人碰撞体
      var colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius,enemyLayerMask);
      foreach (var collider in colliders)
      {
         //取得敌人脚本
         if (collider.TryGetComponent<Enemy>(out Enemy enemy))
         {
            //调用敌人受伤函数
            enemy.TakeDamage(explosionDamage);
         }
      }
   }
   
   //绘制简单几何模型
   private void OnDrawGizmosSelected()
   {
      //设置线条颜色
      Gizmos.color = Color.yellow;
      //画出一个线框球体
      Gizmos.DrawWireSphere(transform.position,explosionRadius);
   }
   //变速协程
   IEnumerator VariableSpeedCoroutine()
   {
      //移动速度先低速
      moveSpeed = lowSpeed;
      //挂起等待一段时间
      yield return _waitVariableSpeedDelay;
      //移动速度再为高速
      moveSpeed = highSpeed;
      //如果目标不为空，播放一段特定音效
      if (target != null)
         AudioManager.Instance.PlayerRandomSFX(targetAcquiredVoice);
   }
}
