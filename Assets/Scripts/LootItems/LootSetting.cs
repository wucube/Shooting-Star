using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战利品设置
/// </summary>
[System.Serializable] public class LootSetting
{
    //游戏战利品对象
    public GameObject prefab;
    //战利品掉率百分比
    [Range(0f, 100f)] public float dropPercentage;

    //生成战利品道具功能 接收三维向量参数，设置道具生成位置
    public void Spawn(Vector3 position)
    {
        //若随机值小于等于掉率值
        if (Random.Range(0f, 100f) <= dropPercentage)
        {
            //对象池管理器生成一个战利品道具
            PoolManager.Release(prefab, position);
        }
    }
}
