using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    //��������������رյľ�̬ί��
    public static UnityAction on  =delegate { };
    public static UnityAction off = delegate { };

    //�����������Ӿ���Ч
    [SerializeField] private GameObject triggerVFX;
    [SerializeField] private GameObject engineVFXNormal;
    [SerializeField] private GameObject engineVFXOverdrive;
    //��������������رյ���Ч
    [SerializeField] private AudioData onSFX;
    [SerializeField] private AudioData offSFX;
    
    private void Awake()
    {
        //����on��offί��
        on += On;
        off += Off;
    }
    //�ű��������ڵĿ�ʼ��������ġ��˶�ί�У���Ϊί�������Ա������������
    private void OnDestroy()
    {
        //�˶�on��offί��
        on -= On;
        off -= Off;
    }
    //onί�д�����
    void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayerRandomSFX(onSFX);
    }
    //offί�д�����
    void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayerRandomSFX(offSFX);
    }
}
