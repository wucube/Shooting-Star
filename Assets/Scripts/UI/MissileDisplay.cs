using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    //导弹数量文本组件
    private static Text amountText;
    //冷却图片组件
    private static Image cooldownImage;
    private void Awake()
    {
        //获取子对象文本组件
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        //获取子对象冷却图片组件
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
    }
    //更新显示导弹数量
    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
    //更新显示冷却图片
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}