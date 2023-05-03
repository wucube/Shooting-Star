using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分数奖励战利品
/// </summary>
public class ScoreBonusPickUp :LootItem
{
   [SerializeField] private int scoreBonus;
  
   protected override void PickUp()
   {
      ScoreManager.Instance.AddScore(scoreBonus);
      base.PickUp();
   }
}
