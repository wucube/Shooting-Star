using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器威力提升
/// </summary>
public class WeaponPowerPickUp : LootItem
{
    /// <summary>
    /// 满级子弹时的声效
    /// </summary>
    [SerializeField] private AudioData fullPowerPickUpSFX;
    /// <summary>
    /// 满级子弹时拾取升级子弹战利品的奖励
    /// </summary>
    [SerializeField] private int fullPowerScoreBonus = 200;

    /// <summary>
    /// 拾取子弹升级战利品
    /// </summary>
    protected override void PickUp()
    {
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = "POWER UP!";
            player.PowerUp();
        }
        
        base.PickUp();
    }
}
