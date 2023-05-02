using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
   //��Ѫʱ��ʰȡ������Ч
   [SerializeField] private AudioData fullHealthPickUpSFX;
   //��Ѫʱ�Ļָ��������
   [SerializeField] private int fullHealthScoreBonus = 200;
   //δ��Ѫʱ��Ѫ���ָ�
   [SerializeField] private float shieldBonus = 20f;
   protected override void PickUp()
   {
      //�����Ѫ��ȫ��
      if (player.IsFullHealth)
      {
         //������Ѫʱ��ʰȡ������Ч
         pickUpSFX = fullHealthPickUpSFX;
         //ʰȡ�ı�Ϊ���ӷ���
         lootMessage.text = $"SCORE + {fullHealthScoreBonus}";
         //������ҷ���
         ScoreManager.Instance.AddScore(fullHealthScoreBonus);
      }
      else //���Ѫ��δ��
      {
         //����Ĭ����Ч
         pickUpSFX = defaultPickUpSFX;
         //ʰȡ�ı�Ϊ����ֵ�ָ�
         lootMessage.text = $"SHIELD + {shieldBonus}";
         //�ָ����Ѫ��
         player.RestoreHealth(shieldBonus); 
      }
      base.PickUp();
   }
}
