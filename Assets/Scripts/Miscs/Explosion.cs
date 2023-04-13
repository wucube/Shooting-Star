using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
   //爆炸伤害值
   [SerializeField] private float explosionDamage = 100f;
   //爆炸范围碰撞体
   [SerializeField] private Collider2D explosionCollider;
   //等待挂起时间
   private WaitForSeconds _waitExplosionTime = new WaitForSeconds(0.1f);

   private void OnEnable()
   {
      //启用爆炸协程
      StartCoroutine(ExplosionCoroutine());
   }
   //触发器函数
   private void OnTriggerEnter2D(Collider2D other)
   {
      //如果获取到敌人碰撞体上的敌人脚本
      if (other.TryGetComponent(out Enemy enemy))
      {
         //调用敌人受伤函数
         enemy.TakeDamage(explosionDamage);
      }
   }
   //爆炸范围协程
   IEnumerator ExplosionCoroutine()
   {
      //爆炸范围触发器启用
      explosionCollider.enabled = true;
      //挂起等待0.1秒
      yield return _waitExplosionTime;
      //触发器关闭
      explosionCollider.enabled = false;
   }
}
