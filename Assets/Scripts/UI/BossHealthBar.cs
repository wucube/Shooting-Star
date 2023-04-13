using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatsBar_HUD
{
    //重写设置文本百分比函数，显示小数点后两位
    protected override void SetPercentText()
    {
        // P即percentage，2 代表小数位数为2
        percentText.text = targetFillAmount.ToString("P2");
    }
}
