using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理器
/// </summary>
public class PoolManager: MonoBehaviour
{
    [SerializeField] private Pool[] enemyPools;
    [SerializeField] private Pool[] playerProjectilePools;
    [SerializeField] private Pool[] enemyProjectilePools;
    [SerializeField] private Pool[] vFXPools;
    [SerializeField] private Pool[] lootItemPools;

    /// <summary>
    /// 关联对象和对象池的容器
    /// </summary>
    static Dictionary<GameObject, Pool> dictionary;
    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        
        Initialize(enemyPools);
        
        Initialize(playerProjectilePools);
        
        Initialize(enemyProjectilePools);
       
        Initialize(vFXPools);
        
        Initialize(lootItemPools);
    }
#if UNITY_EDITOR
    
    void OnDestroy()
    {
    
        CheckPoolSize(enemyPools);
        
        CheckPoolSize(playerProjectilePools);
        
        CheckPoolSize(enemyProjectilePools);
        
        CheckPoolSize(vFXPools);
        
        CheckPoolSize(lootItemPools);
        
    }
#endif
    
    /// <summary>
    /// 检查对象池运行时的尺寸
    /// </summary>
    /// <param name="pools"></param>
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
    /// 初始化对象池
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
           
            Transform poolParent =  new GameObject("Pool:" + pool.Prefab.name).transform;
           
            poolParent.parent = transform;
           
            pool.Initialize(poolParent);
        }    
    }
    /// <summary>
    /// 对象池释放对象
    /// </summary>
    /// <param name="prefab"></param>
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

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation,localScale);
    }
}
