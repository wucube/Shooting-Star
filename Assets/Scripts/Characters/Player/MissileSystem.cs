using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    //导弹默认数量
    [SerializeField] private int defaultAmount = 5;
    //导弹发射冷却时间
    [SerializeField] private float cooldownTime = 1f;
    
    //导弹预制体
    [SerializeField] private GameObject missilePrefab = null;
    //导弹发射音效变量
    [SerializeField] private AudioData launchSFX = null;
    
    //导弹是否准备好
    private bool isReady = true;
    
    //当前拥有的导弹数量
    private int _amount;

    private void Awake()
    {
        //默认导弹数量赋予当前导弹数量
        _amount = defaultAmount;
    }

    private void Start()
    {
        //调用导弹数量更新函数
        MissileDisplay.UpdateAmountText(_amount);
    }

    //导弹拾取函数
    public void PickUp()
    {
        //导弹数量自增
        _amount++;
        //更新显示导弹文本UI 
        MissileDisplay.UpdateAmountText(_amount);
        //如果先前导弹数量为0，拾取导弹后数量为1，
        if (_amount == 1)
        {
            //更新显示导弹图标UI 
            MissileDisplay.UpdateCooldownImage(0f);
            //导弹发射已准备好
            isReady = true;
        }
    }
    //导弹发射函数
    public void Launch(Transform muzzleTransform)
    {
        //如果当前导弹数量为0，直接返回
        if(_amount ==0 ||!isReady) return; // 导弹数量为0，还可以播放错误音效，UI做出一定改变等
        //导弹还没有准备好
        isReady = false;
        
        //对象池管理器生成导弹
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //播放导弹发射音效
        AudioManager.Instance.PlayerRandomSFX(launchSFX);
        
        //当前导弹数量减少一个
        _amount--;
        //更新显示导弹UI
        MissileDisplay.UpdateAmountText(_amount);
        //导弹数量变为0，则更新显示冷却图片函数
        if (_amount == 0) MissileDisplay.UpdateCooldownImage(1f);
        //导弹数量不为0，进入冷却状态
        else StartCoroutine(CooldownCoroutine());
    }
    
    //冷却协程
    IEnumerator CooldownCoroutine()
    {
        //存储导弹冷却时间
        var cooldownValue = cooldownTime;
        //冷却值大于0进入循环
        while (cooldownValue > 0f)
        {
            //更新冷却图片填充值，冷却值介于0到1之间
            MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);
            //冷却值不断减少，且永不小于0
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);
            //挂起等待下一帧
            yield return null;
        }
        //导弹发射功能准备好
        isReady = true;
    }
}