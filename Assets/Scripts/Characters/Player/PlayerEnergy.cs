using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    //能量条类引用变量
    [SerializeField] private EnergyBar energyBar;
    //能量爆发间隔时间
    [SerializeField] private float overdriveInterval = 0.1f;
    //能否获取能量
    private bool _available = true;
    //能量值最大值常量，不会改变
    public const int Max = 100;
    //能量百分比值常量
    public const int Percent = 1;
    //存储当前能量值
    private int _energy;
    //挂起等待能量爆发间隔时间
    private WaitForSeconds _waitForOverdriveInterval;
    protected override void Awake()
    {
        base.Awake();
        //挂起等待能量爆发时间间隔初始化
        _waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    void OnEnable()
    {
        //订阅能量爆发系统的On与Off委托
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        //退订能量爆发系统的On与Off委托
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    void Start()
    {
        //能量条UI初始化,传入当前能量值与最大能量值
        energyBar.Initialize(_energy,Max);
        //玩家能量值全满
        Obtain(Max);
    }
    //获取能量函数
    public void Obtain(int value)
    {
        //若能量值是满，或处于能量爆发状态或玩家死亡，则提前返回
        if (_energy == Max || !_available||!gameObject.activeSelf) return;
        //能量值不满则累加获取能量，同时防止能量值溢出
        _energy = Mathf.Clamp(_energy + value, 0, Max);
        //更新能量条的UI显示
        energyBar.UpdateStats(_energy,Max);
    }
    //能量消耗函数
    public void Use(int value)
    {
        //当前能量值 减 消耗能量值
        _energy -= value;
        //更新能量条的UI显示
        energyBar.UpdateStats(_energy,Max);
        //处于能量爆发状态即不能获取能量时，且能量值消耗完毕时，关闭能量爆发
        if(_energy==0&&!_available) PlayerOverdrive.off.Invoke();
    }
    //判断当前能量值是否足够支付消耗的能量值
    public bool IsEnough(int value) => _energy >= value;

   //开启能量爆发委托处理函数
    void PlayerOverdriveOn()
    {
        //不能获取能量
        _available = false;
        //开启持续消耗协程
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    //关闭能量爆发委托处理函数
    void PlayerOverdriveOff()
    {
        //可以获取能量
        _available = true;
        //停止持续消耗协程
        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    //持续消耗协程
    IEnumerator KeepUsingCoroutine()
    {
        //玩家活着且能量大于0时，持续循环
        while (gameObject.activeSelf&&_energy>0)
        {
            //挂起等待能量爆发间隔时间，每过0.1秒
            yield return _waitForOverdriveInterval;
            //消耗能量值，1个百分比
            Use(Percent);
        }
    }
}
