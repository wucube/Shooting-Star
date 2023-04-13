using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 导弹显示
/// </summary>
public class MissileDisplay : MonoBehaviour
{
    /// <summary>
    ///  导弹剩余数量的文本
    /// </summary>
    private static Text amountText;
    /// <summary>
    /// 导弹冷却图
    /// </summary>
    private static Image cooldownImage;

    private void Awake()
    {
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
    }
    /// <summary>
    /// 更新导弹剩余数量
    /// </summary>
    /// <param name="amount"></param>
    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
    /// <summary>
    /// 更新导弹冷却图片的填充值，为0表示冷却完成，导弹可以发射。
    /// </summary>
    /// <param name="fillAmount"></param>
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}