using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        //��̬����Ϊ�գ���ֵ������
        if (Instance == null) Instance = this as T;
        //��̬���Բ�Ϊ�գ��Ҳ�����������࣬����ԭ���ű����صĶ���
        else if (Instance!=this) Destroy(gameObject);
        //�����³���ʱ�����ݻٸýű����ض���
        DontDestroyOnLoad(gameObject);
    }
}
