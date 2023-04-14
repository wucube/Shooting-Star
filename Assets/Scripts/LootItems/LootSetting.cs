using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战利品设置类
/// </summary>
[System.Serializable] 
public class LootSetting
{
    /// <summary>
    /// 战利品对象预制体
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// 掉落百分比概念
    /// </summary>
    [Range(0f, 100f)] public float dropPercentage;

    /// <summary>
    /// 战利品生成
    /// </summary>
    /// <param name="position">生成位置</param>
    public void Spawn(Vector3 position)
    {
        //随机数小于掉落概念就生成战利品
        if (Random.Range(0f, 100f) <= dropPercentage)
            PoolManager.Release(prefab, position);
    }
}
