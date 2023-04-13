using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战利品生成类
/// </summary>
public class LootSpawner : MonoBehaviour
{
    /// <summary>
    /// 战利品生成设置数组
    /// </summary>
    [SerializeField] private LootSetting[] lootSettings;

    /// <summary>
    /// 战利品生成
    /// </summary>
    /// <param name="position"></param>
    public void Spawn(Vector2 position)
    {
        //遍历战利品生成设置数组
        foreach (var item in lootSettings)
        {
            //随机生成战利品，每个战利品位置略有位置偏差
            item.Spawn(position + Random.insideUnitCircle);
        }
    }
}
