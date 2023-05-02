using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
    //武器威力满级时的播放的音效
    [SerializeField] private AudioData fullPowerPickUpSFX;
    //分数奖励
    [SerializeField] private int fullPowerScoreBonus = 200;

    protected override void PickUp()
    {
        //若玩家武器威力满级，
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
            //增加玩家分数
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
        }
        else//若玩家武器威力未满
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = "POWER UP!";
            //提升玩家武器威力
            player.PowerUp();
        }
        base.PickUp();
    }
}
