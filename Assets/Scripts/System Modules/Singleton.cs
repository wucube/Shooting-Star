using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泛型单例类
/// </summary>
/// <typeparam name="T">泛型类必须继承组件基类</typeparam>
public class Singleton<T> : MonoBehaviour where T:Component
{  
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
