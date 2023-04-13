using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家能量系统
/// </summary>
public class PlayerEnergy : Singleton<PlayerEnergy>
{
    /// <summary>
    /// 玩家能量条
    /// </summary>
    [SerializeField] private EnergyBar energyBar;

    /// <summary>
    /// 能量爆发延迟时间
    /// </summary>
    [SerializeField] private float overdriveInterval = 0.1f;
   
    /// <summary>
    /// 能否获取能量
    /// </summary>
    private bool available = true;

    /// <summary>
    /// 能量最大值
    /// </summary>
    public const int Max = 100;

    /// <summary>
    /// 能量百分比
    /// </summary>
    public const int Percent = 1;
    
    /// <summary>
    /// 当前能量值
    /// </summary>
    private int energy;

    /// <summary>
    /// 能量爆发状态下的能量扣除时间间隔
    /// </summary>
    private WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    void Start()
    {
        energyBar.Initialize(energy,Max);
        Obtain(Max);
    }
    
    /// <summary>
    /// 获取能量
    /// </summary>
    /// <param name="value">增加的能量值</param>
    public void Obtain(int value)
    {
        //若当前能量值满 或 能量不可获取 或 对象没有激活 就直接返回
        if (energy == Max || !available || !gameObject.activeSelf) return;
        
        energy = Mathf.Clamp(energy + value, 0, Max);
        energyBar.UpdateStats(energy, Max);
    }

    /// <summary>
    /// 能量使用
    /// </summary>
    /// <param name="value">扣除的能量值</param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStats(energy,Max);

        //能量值为0 且 能量不可获取 就启用能量爆发关闭事件
        if(energy == 0 && !available ) PlayerOverdrive.off.Invoke();
    }
    
    /// <summary>
    /// 能量是否足够
    /// </summary>
    /// <param name="value">要使用的能量值</param>
    /// <returns></returns>
    public bool IsEnough(int value) => energy >= value;

    /// <summary>
    /// 能量爆发开启事件处理器
    /// </summary>
    void PlayerOverdriveOn()
    {
        //不能获取能量
        available = false;

        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    
    /// <summary>
    /// 能量爆发关闭事件处理器
    /// </summary>
    void PlayerOverdriveOff()
    {
        //可以获取能量
        available = true;

        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    
    /// <summary>
    /// 持续使用能量值
    /// </summary>
    /// <returns></returns>
    IEnumerator KeepUsingCoroutine()
    {
        while ( gameObject.activeSelf && energy > 0 )
        {
            yield return waitForOverdriveInterval;
            
            Use(Percent);
        }
    }
}
