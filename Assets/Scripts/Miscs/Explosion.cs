using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
   //��ը�˺�ֵ
   [SerializeField] private float explosionDamage = 100f;
   //��ը��Χ��ײ��
   [SerializeField] private Collider2D explosionCollider;
   //�ȴ�����ʱ��
   private WaitForSeconds _waitExplosionTime = new WaitForSeconds(0.1f);

   private void OnEnable()
   {
      //���ñ�ըЭ��
      StartCoroutine(ExplosionCoroutine());
   }
   //����������
   private void OnTriggerEnter2D(Collider2D other)
   {
      //�����ȡ��������ײ���ϵĵ��˽ű�
      if (other.TryGetComponent(out Enemy enemy))
      {
         //���õ������˺���
         enemy.TakeDamage(explosionDamage);
      }
   }
   //��ը��ΧЭ��
   IEnumerator ExplosionCoroutine()
   {
      //��ը��Χ����������
      explosionCollider.enabled = true;
      //����ȴ�0.1��
      yield return _waitExplosionTime;
      //�������ر�
      explosionCollider.enabled = false;
   }
}
