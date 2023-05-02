using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    //����Ĭ������
    [SerializeField] private int defaultAmount = 5;
    //����������ȴʱ��
    [SerializeField] private float cooldownTime = 1f;
    
    //����Ԥ����
    [SerializeField] private GameObject missilePrefab = null;
    //����������Ч����
    [SerializeField] private AudioData launchSFX = null;
    
    //�����Ƿ�׼����
    private bool isReady = true;
    
    //��ǰӵ�еĵ�������
    private int _amount;

    private void Awake()
    {
        //Ĭ�ϵ����������赱ǰ��������
        _amount = defaultAmount;
    }

    private void Start()
    {
        //���õ����������º���
        MissileDisplay.UpdateAmountText(_amount);
    }

    //����ʰȡ����
    public void PickUp()
    {
        //������������
        _amount++;
        //������ʾ�����ı�UI 
        MissileDisplay.UpdateAmountText(_amount);
        //�����ǰ��������Ϊ0��ʰȡ����������Ϊ1��
        if (_amount == 1)
        {
            //������ʾ����ͼ��UI 
            MissileDisplay.UpdateCooldownImage(0f);
            //����������׼����
            isReady = true;
        }
    }
    //�������亯��
    public void Launch(Transform muzzleTransform)
    {
        //�����ǰ��������Ϊ0��ֱ�ӷ���
        if(_amount ==0 ||!isReady) return; // ��������Ϊ0�������Բ��Ŵ�����Ч��UI����һ���ı��
        //������û��׼����
        isReady = false;
        
        //����ع��������ɵ���
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        //���ŵ���������Ч
        AudioManager.Instance.PlayerRandomSFX(launchSFX);
        
        //��ǰ������������һ��
        _amount--;
        //������ʾ����UI
        MissileDisplay.UpdateAmountText(_amount);
        //����������Ϊ0���������ʾ��ȴͼƬ����
        if (_amount == 0) MissileDisplay.UpdateCooldownImage(1f);
        //����������Ϊ0��������ȴ״̬
        else StartCoroutine(CooldownCoroutine());
    }
    
    //��ȴЭ��
    IEnumerator CooldownCoroutine()
    {
        //�洢������ȴʱ��
        var cooldownValue = cooldownTime;
        //��ȴֵ����0����ѭ��
        while (cooldownValue > 0f)
        {
            //������ȴͼƬ���ֵ����ȴֵ����0��1֮��
            MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);
            //��ȴֵ���ϼ��٣�������С��0
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);
            //����ȴ���һ֡
            yield return null;
        }
        //�������书��׼����
        isReady = true;
    }
}
