using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    //能量爆发开启与关闭的静态委托
    public static UnityAction on  =delegate { };
    public static UnityAction off = delegate { };

    //能量爆发的视觉特效
    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverdrive;
    //能量爆发开启与关闭的音效
    [SerializeField] private AudioData onSFX;
    [SerializeField] private AudioData offSFX;
    
    private void Awake()
    {
        //订阅on与off委托
        on += On;
        off += Off;
    }
    //脚本生命周期的开始与结束订阅、退订委托，因为委托是类成员，可以这样做
    private void OnDestroy()
    {
        //退订on与off委托
        on -= On;
        off -= Off;
    }
    //on委托处理函数
    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayerRandomSFX(onSFX);
    }
    //off委托处理函数
    void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayerRandomSFX(offSFX);
    }
}
