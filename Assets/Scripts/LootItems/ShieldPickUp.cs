using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 护盾(血量)战利品拾取
/// </summary>
public class ShieldPickUp : LootItem
{
   /// <summary>
   /// 满状态时的拾取音效
   /// </summary>
   [SerializeField] private AudioData fullHealthPickUpSFX;
   
   /// <summary>
   /// 满状态时护盾战利品的分数奖励
   /// </summary>
   [SerializeField] private int fullHealthScoreBonus = 200;

   /// <summary>
   /// 护盾(血量)值奖励
   /// </summary>
   [SerializeField] private float shieldBonus = 20f;

   protected override void PickUp()
   {
      if (player.IsFullHealth)
      {
         pickUpSFX = fullHealthPickUpSFX;
         
         lootMessage.text = $"SCORE + {fullHealthScoreBonus}";
      
         ScoreManager.Instance.AddScore(fullHealthScoreBonus);
      }
      else 
      {
         pickUpSFX = defaultPickUpSFX;
         
         lootMessage.text = $"SHIELD + {shieldBonus}";
        
         player.RestoreHealth(shieldBonus); 
      }
      base.PickUp();
   }
}
