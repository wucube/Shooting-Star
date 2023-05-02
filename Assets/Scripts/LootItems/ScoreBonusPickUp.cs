using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBonusPickUp :LootItem
{
   [SerializeField] private int scoreBonus;
   //��дʰȡ����
   protected override void PickUp()
   {
      //���ӷ���
      ScoreManager.Instance.AddScore(scoreBonus);
      base.PickUp();
   }
}
