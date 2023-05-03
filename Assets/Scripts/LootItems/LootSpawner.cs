using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战利品生成
/// </summary>
public class LootSpawner : MonoBehaviour
{
    //战利品设置数组
    [SerializeField] private LootSetting[] lootSettings;

    //生成战利品函数 接收三维向量设置战利品设置
    public void Spawn(Vector2 position)
    {
        //遍历战利品设置数组
        foreach (var item in lootSettings)
        {
            //调用数组中每个战利品设置的生成函数，道具生成产生一点偏移
            item.Spawn(position+Random.insideUnitCircle);
        }
    }
}
