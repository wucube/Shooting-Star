using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss血条类
/// </summary>
public class BossHealthBar : StatsBar_HUD
{
    //设置剩余血量的百分比文本
    protected override void SetPercentText()
    {
        percentText.text = targetFillAmount.ToString("P2");//小数转字符串，保留小数两位
    }
}
