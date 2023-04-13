using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp :LootItem
{
   [SerializeField] private int scoreBonus;
   //重写拾取函数
   protected override void PickUp()
   {
      //增加分数
      ScoreManager.Instance.AddScore(scoreBonus);
      base.PickUp();
   }
}
