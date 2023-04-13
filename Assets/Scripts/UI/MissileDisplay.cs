using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    //���������ı����
    private static Text amountText;
    //��ȴͼƬ���
    private static Image cooldownImage;
    private void Awake()
    {
        //��ȡ�Ӷ����ı����
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        //��ȡ�Ӷ�����ȴͼƬ���
        cooldownImage = transform.Find("Cooldown Image").GetComponent<Image>();
    }
    //������ʾ��������
    public static void UpdateAmountText(int amount) => amountText.text = amount.ToString();
    //������ʾ��ȴͼƬ
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}