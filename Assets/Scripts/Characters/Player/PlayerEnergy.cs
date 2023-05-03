using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家能量系统
/// </summary>
public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar energyBar;

    /// <summary>
    /// 能量爆发间隔
    /// </summary>
    [SerializeField] private float overdriveInterval = 0.1f;

    /// <summary>
    /// 能否获得能量
    /// </summary>
    private bool available = true;

    public const int Max = 100;

    public const int Percent = 1;

    /// <summary>
    /// 玩家当前能量
    /// </summary>
    private int energy;
    
    private WaitForSeconds _waitForOverdriveInterval;
    protected override void Awake()
    {
        base.Awake();
       
        _waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
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
    /// 能量取得
    /// </summary>
    /// <param name="value"></param>
    public void Obtain(int value)
    {
        if (energy == Max || !available||!gameObject.activeSelf) return;

        energy = Mathf.Clamp(energy + value, 0, Max);
        
        energyBar.UpdateStats(energy,Max);
    }
    
    /// <summary>
    /// 能量使用
    /// </summary>
    /// <param name="value"></param>
    public void Use(int value)
    {
        energy -= value;

        energyBar.UpdateStats(energy,Max);

        if(energy==0&&!available) PlayerOverdrive.off.Invoke();
    }

    /// <summary>
    /// 能量是否足够使用
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsEnough(int value) => energy >= value;

    /// <summary>
    /// 能量爆发开启
    /// </summary>
    void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    /// <summary>
    /// 能量爆发关闭
    /// </summary>
    void PlayerOverdriveOff()
    {
        available = true;

        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    
    /// <summary>
    /// 持续使用能量调用
    /// </summary>
    /// <returns></returns>
    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return _waitForOverdriveInterval;

            Use(Percent);
        }
    }
}
