using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
   //满血时的拾取道具音效
   [SerializeField] private AudioData fullHealthPickUpSFX;
   //满血时的恢复奖励变更
   [SerializeField] private int fullHealthScoreBonus = 200;
   //未满血时的血量恢复
   [SerializeField] private float shieldBonus = 20f;
   protected override void PickUp()
   {
      //若玩家血量全满
      if (player.IsFullHealth)
      {
         //播放满血时的拾取道具音效
         pickUpSFX = fullHealthPickUpSFX;
         //拾取文本为增加分数
         lootMessage.text = $"SCORE + {fullHealthScoreBonus}";
         //增加玩家分类
         ScoreManager.Instance.AddScore(fullHealthScoreBonus);
      }
      else //玩家血量未满
      {
         //播放默认音效
         pickUpSFX = defaultPickUpSFX;
         //拾取文本为护盾值恢复
         lootMessage.text = $"SHIELD + {shieldBonus}";
         //恢复玩家血量
         player.RestoreHealth(shieldBonus); 
      }
      base.PickUp();
   }
}
