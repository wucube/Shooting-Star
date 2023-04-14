using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分数奖励战利品拾取
/// </summary>
public class ScoreBonusPickUp :LootItem
{
   /// <summary>
   /// 分数奖励
   /// </summary>
   [SerializeField] private int scoreBonus;
   
   protected override void PickUp()
   {
      //添加分数
      ScoreManager.Instance.AddScore(scoreBonus);

      base.PickUp();
   }
}
