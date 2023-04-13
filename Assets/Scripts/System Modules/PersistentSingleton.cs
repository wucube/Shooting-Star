using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承Mono的泛型持久单例
/// </summary>
/// <typeparam name="T">泛型类必须继承组件基类</typeparam>
public class PersistentSingleton<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        //如果实例字段值为空，就将基类赋值给字段，并转为派生类
        if (Instance == null) 
            Instance = this as T;
        //如果实例字段已经有值，就不需要赋值，销毁自己即可。
        else if (Instance!=this) 
            Destroy(gameObject);
        //过场景不移除
        DontDestroyOnLoad(gameObject);
    }
}
