using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD状态条
/// </summary>
public class StatsBar_HUD : StatsBar
{
    /// <summary>
    /// 百分比数字显示的文本
    /// </summary>
    [SerializeField] protected Text percentText;
    
    /// <summary>
    /// 设置百分比文本
    /// </summary>
    protected virtual void SetPercentText()
    {
        percentText.text = targetFillAmount.ToString("P0");

    }
    /// <summary>
    /// 初始化状态条
    /// </summary>
    /// <param name="currentValue">当前值</param>
    /// <param name="maxValue">最大值</param>
    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }
    
    /// <summary>
    /// 状态条延缓变化的协程
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        
        return base.BufferedFillingCoroutine(image);
    }
}
