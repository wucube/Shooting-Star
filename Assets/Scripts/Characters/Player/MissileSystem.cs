using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 导弹系统
/// </summary>
public class MissileSystem : MonoBehaviour
{
    /// <summary>
    /// 导弹的默认数量
    /// </summary>
    [SerializeField] private int defaultAmount = 5;

    /// <summary>
    /// 导弹发射的冷却时间
    /// </summary>
    [SerializeField] private float cooldownTime = 1f;
    
    /// <summary>
    /// 导弹预制体
    /// </summary>
    [SerializeField] private GameObject missilePrefab = null;

    /// <summary>
    /// 导弹发射声效
    /// </summary>
    [SerializeField] private AudioData launchSFX = null;
    
    /// <summary>
    /// 导弹发射是否准备完成
    /// </summary>
    private bool isReady = true;
    
    /// <summary>
    /// 导弹剩余数量
    /// </summary>
    private int amount;

    private void Awake()
    {
        amount = defaultAmount;
    }

    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }

    /// <summary>
    /// 导弹拾取
    /// </summary>
    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateAmountText(amount);

        //剩余导弹数量为0，表示之前导弹已发射完。此时拾取新导弹后直接进入可发射状态。
        if (amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }

    /// <summary>
    /// 导弹发射
    /// </summary>
    /// <param name="muzzleTransform">枪口位置</param>
    public void Launch(Transform muzzleTransform)
    {
        //导弹剩余数量为0 或 导弹未准备完成，直接返回
        if(amount ==0 ||!isReady) return; 

        //下次的导弹发射未准备好
        isReady = false;
        
        //对象释放导弹对象
        PoolManager.Release(missilePrefab, muzzleTransform.position);

        AudioManager.Instance.PlayerRandomSFX(launchSFX);
        amount--;
        MissileDisplay.UpdateAmountText(amount);

        if (amount == 0) 
            MissileDisplay.UpdateCooldownImage(1f);
        else 
            StartCoroutine(CooldownCoroutine());
    }
    
    /// <summary>
    /// 导弹冷却协程
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCoroutine()
    {
        var cooldownValue = cooldownTime;

        //若导弹冷却时间值大于0
        while (cooldownValue > 0f)
        {
            //每帧改变导弹冷却图片的填充值
            MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);

            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }
        //导弹发射准备完成
        isReady = true;
    }
}
