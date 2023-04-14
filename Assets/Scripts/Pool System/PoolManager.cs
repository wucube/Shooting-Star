using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理器
/// </summary>
public class PoolManager: MonoBehaviour
{
    [SerializeField] private Pool[] enemyPools;

    /// <summary>
    /// 玩家子弹池
    /// </summary>
    [SerializeField] private Pool[] playerProjectilePools;

    /// <summary>
    /// 敌人子弹池
    /// </summary>
    [SerializeField] private Pool[] enemyProjectilePools;
    
    /// <summary>
    /// 视效池
    /// </summary>
    [SerializeField] private Pool[] vfxPools;

    /// <summary>
    /// 战利品池
    /// </summary>
    [SerializeField] private Pool[] lootItemPools;
    
    /// <summary>
    /// 对象和其所对应池子的字典
    /// </summary>
    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vfxPools);
        Initialize(lootItemPools);
    }
    #if UNITY_EDITOR
    
    void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vfxPools);
        CheckPoolSize(lootItemPools);
    }
    #endif
    
    /// <summary>
    /// 检测运行时对象池的实际容量
    /// </summary>
    /// <param name="pools">池数组</param>
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool:{0} has a runtime size{1} bigger than its inital size{2}!",
                        pool.Prefab.name,
                        pool.RuntimeSize,
                        pool.Size));
            }
        }
    }
    /// <summary>
    /// 初始化对象池数组
    /// </summary>
    /// <param name="pools"></param>
    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR  
           
            if (dictionary.ContainsKey(pool.Prefab))
            {
                
                Debug.LogError("Same prefab in multiple pools! Prefab:"+pool.Prefab.name);
                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);
            
            //池中对象的父对象
            Transform poolParent =  new GameObject("Pool:" + pool.Prefab.name).transform;
            
            //池中对象的父对象的父对象为池管理器
            poolParent.parent = transform;
            
            pool.Initialize(poolParent);
        }    
    }
    /// <summary>
    /// 释放对象
    /// </summary>
    /// <param name="prefab">对象预制体</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR

        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    /// <param name="prefab">对象预制体</param>
    /// <param name="position">对象位置</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:"+prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    /// <param name="prefab">对象预制体</param>
    /// <param name="position">对象位置</param>
    /// <param name="rotation">对象旋转</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position,rotation);
    }
    
    /// <summary>
    /// 释放对象
    /// </summary>
    /// <param name="prefab">对象预制体</param>
    /// <param name="position">对象位置</param>
    /// <param name="rotation">对象旋转</param>
    /// <param name="localScale">对象缩放</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
