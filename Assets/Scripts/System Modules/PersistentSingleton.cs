using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        //静态属性为空，则赋值泛型类
        if (Instance == null) Instance = this as T;
        //静态属性不为空，且不是这个泛型类，销毁原来脚本挂载的对象
        else if (Instance!=this) Destroy(gameObject);
        //加载新场景时，不摧毁该脚本挂载对象
        DontDestroyOnLoad(gameObject);
    }
}
