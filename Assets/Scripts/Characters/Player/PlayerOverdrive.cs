using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 玩家能量爆发
/// </summary>
public class PlayerOverdrive : MonoBehaviour
{
    /// <summary>
    /// 静态能量爆发开启事件
    /// </summary>
    public static UnityAction on = delegate { };

    /// <summary>
    /// 静态能量爆发关闭事件
    /// </summary>
    public static UnityAction off = delegate { };
    
    /// <summary>
    /// 能量爆发启动视效
    /// </summary>
    [SerializeField] private GameObject triggerVFX;

    /// <summary>
    /// 机体引擎火焰的正常视效
    /// </summary>
    [SerializeField] private GameObject engineVFXNormal;

    /// <summary>
    /// 机体引擎火焰的能量爆发视效
    /// </summary>
    [SerializeField] private GameObject engineVFXOverdrive;

    /// <summary>
    /// 能量爆发开启的音频数量
    /// </summary>
    [SerializeField] private AudioData onSFX;

    /// <summary>
    /// 能量爆发关闭的音频数量
    /// </summary>
    [SerializeField] private AudioData offSFX;
    
    private void Awake()
    {
        on += On;
        off += Off;
    }

    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

    /// <summary>
    /// 能量爆发开启的事件处理器
    /// </summary>
    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayerRandomSFX(onSFX);
    }

    /// <summary>
    /// 能量爆发关闭的事件处理器
    /// </summary>
    void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayerRandomSFX(offSFX);
    }
}
