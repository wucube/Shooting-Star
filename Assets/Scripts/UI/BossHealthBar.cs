using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatsBar_HUD
{
    //��д�����ı��ٷֱȺ�������ʾС�������λ
    protected override void SetPercentText()
    {
        // P��percentage��2 ����С��λ��Ϊ2
        percentText.text = targetFillAmount.ToString("P2");
    }
}
