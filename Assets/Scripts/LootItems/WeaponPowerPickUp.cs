using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
    //������������ʱ�Ĳ��ŵ���Ч
    [SerializeField] private AudioData fullPowerPickUpSFX;
    //��������
    [SerializeField] private int fullPowerScoreBonus = 200;

    protected override void PickUp()
    {
        //�������������������
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
            //������ҷ���
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
        }
        else//�������������δ��
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = "POWER UP!";
            //���������������
            player.PowerUp();
        }
        base.PickUp();
    }
}
