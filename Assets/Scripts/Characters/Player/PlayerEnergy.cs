using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    //�����������ñ���
    [SerializeField] private EnergyBar energyBar;
    //�����������ʱ��
    [SerializeField] private float overdriveInterval = 0.1f;
    //�ܷ��ȡ����
    private bool _available = true;
    //����ֵ���ֵ����������ı�
    public const int Max = 100;
    //�����ٷֱ�ֵ����
    public const int Percent = 1;
    //�洢��ǰ����ֵ
    private int _energy;
    //����ȴ������������ʱ��
    private WaitForSeconds _waitForOverdriveInterval;
    protected override void Awake()
    {
        base.Awake();
        //����ȴ���������ʱ������ʼ��
        _waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    void OnEnable()
    {
        //������������ϵͳ��On��Offί��
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        //�˶���������ϵͳ��On��Offί��
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    void Start()
    {
        //������UI��ʼ��,���뵱ǰ����ֵ���������ֵ
        energyBar.Initialize(_energy,Max);
        //�������ֵȫ��
        Obtain(Max);
    }
    //��ȡ��������
    public void Obtain(int value)
    {
        //������ֵ������������������״̬���������������ǰ����
        if (_energy == Max || !_available||!gameObject.activeSelf) return;
        //����ֵ�������ۼӻ�ȡ������ͬʱ��ֹ����ֵ���
        _energy = Mathf.Clamp(_energy + value, 0, Max);
        //������������UI��ʾ
        energyBar.UpdateStats(_energy,Max);
    }
    //�������ĺ���
    public void Use(int value)
    {
        //��ǰ����ֵ �� ��������ֵ
        _energy -= value;
        //������������UI��ʾ
        energyBar.UpdateStats(_energy,Max);
        //������������״̬�����ܻ�ȡ����ʱ��������ֵ�������ʱ���ر���������
        if(_energy==0&&!_available) PlayerOverdrive.off.Invoke();
    }
    //�жϵ�ǰ����ֵ�Ƿ��㹻֧�����ĵ�����ֵ
    public bool IsEnough(int value) => _energy >= value;

   //������������ί�д�����
    void PlayerOverdriveOn()
    {
        //���ܻ�ȡ����
        _available = false;
        //������������Э��
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    //�ر���������ί�д�����
    void PlayerOverdriveOff()
    {
        //���Ի�ȡ����
        _available = true;
        //ֹͣ��������Э��
        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    //��������Э��
    IEnumerator KeepUsingCoroutine()
    {
        //��һ�������������0ʱ������ѭ��
        while (gameObject.activeSelf&&_energy>0)
        {
            //����ȴ������������ʱ�䣬ÿ��0.1��
            yield return _waitForOverdriveInterval;
            //��������ֵ��1���ٷֱ�
            Use(Percent);
        }
    }
}
