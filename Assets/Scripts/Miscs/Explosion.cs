using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆炸范围伤害判定
/// </summary>
public class Explosion : MonoBehaviour
{
   /// <summary>
   /// 爆炸伤害
   /// </summary>
   [SerializeField] private float explosionDamage = 100f;

   /// <summary>
   /// 爆炸碰撞体
   /// </summary>
   [SerializeField] private Collider2D explosionCollider;
   
   /// <summary>
   /// 爆炸判定有效时间
   /// </summary>
   /// <returns></returns>
   private WaitForSeconds waitExplosionTime = new WaitForSeconds(0.1f);

   private void OnEnable()
   {
      StartCoroutine(ExplosionCoroutine());
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent(out Enemy enemy))
         enemy.TakeDamage(explosionDamage);
   }
   /// <summary>
   /// 爆炸协程
   /// </summary>
   /// <returns></returns>
   IEnumerator ExplosionCoroutine()
   {
      explosionCollider.enabled = true;
     
      yield return waitExplosionTime;
      
      explosionCollider.enabled = false;
   }
}
