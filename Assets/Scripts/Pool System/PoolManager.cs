using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager: MonoBehaviour
{
    //敌人对象池数组
    [SerializeField] private Pool[] enemyPools;
    //玩家子弹池数组
    [SerializeField] private Pool[] playerProjectilePools;
    //敌人子弹池数组
    [SerializeField] private Pool[] enemyProjectilePools;
    //视觉特效池数组
    [SerializeField] private Pool[] vFXPools;
    //战利品池数组
    [SerializeField] private Pool[] lootItemPools;
    //关联预制体与对象池
    static Dictionary<GameObject, Pool> dictionary;
    void Awake()
    {
        //初始化字典
        dictionary = new Dictionary<GameObject, Pool>();
        
        //初始化敌人池对象
        Initialize(enemyPools);
        
        //初始化玩家子弹池对象
        Initialize(playerProjectilePools);
        //初始化敌机子弹池对象
        Initialize(enemyProjectilePools);
        //初始化特效池
        Initialize(vFXPools);
        //初始化战利品道具池
        Initialize(lootItemPools);
    }
    #if UNITY_EDITOR
    //编辑器停止运行时自动调用
    void OnDestroy()
    {
        //检查敌人对象池尺寸
        CheckPoolSize(enemyPools);
        
        //检测玩家子弹池尺寸
        CheckPoolSize(playerProjectilePools);
        //检测敌机子弹池尺寸
        CheckPoolSize(enemyProjectilePools);
        //检测特效池尺寸
        CheckPoolSize(vFXPools);
        //检测战利品道具池
        CheckPoolSize(lootItemPools);
        //Debug.Log("已经启用");
    }
    #endif
    
    //检查对象池运行时的尺寸
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            //如果对象池实际运行尺寸大于对象池尺寸 
            if (pool.RuntimeSize > pool.Size)
            {
                //控制台打印警告
                Debug.LogWarning(
                    string.Format("Pool:{0} has a runtime size{1} bigger than its inital size{2}!",
                        pool.Prefab.name,
                        pool.RuntimeSize,
                        pool.Size));
            }
        }
    }
    //初始化所有对象池
    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            //条件编译指令，只在Unity编辑器中运行
        #if UNITY_EDITOR  
            //如果字典的键已有一个相同预制体，直接跳过。字典的每个键必须唯一
            if (dictionary.ContainsKey(pool.Prefab))
            {
                //控制台打印警告信息
                Debug.LogError("Same prefab in multiple pools! Prefab:"+pool.Prefab.name);
                continue;
            }
        #endif
            //对象池的预制体与对象池本身作键值对。每初始化一个对象池，字典就添加一个预制体和池的键值对
            dictionary.Add(pool.Prefab, pool);
            //新建子弹的父对象
            Transform poolParent =  new GameObject("Pool:" + pool.Prefab.name).transform;
            //子弹父对象的父对象为池管理器对象
            poolParent.parent = transform;
            //生成对象池中的所有需要的复制体
            pool.Initialize(poolParent);
        }    
    }
    //静态函数更方便被其他类调用
    /// <summary>
    /// 根据传入对象释放池中预备完成的对象
    /// </summary>
    /// <param name="prefab"></param> 指定释放的对象
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR //条件编译，只在编辑器中运行
        //字典中不包含指定键值时，返回空值
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:" + prefab.name);
            return null;
        }
        #endif
        //根据传入预制体调用对应对象池中的准备对象函数，返回一个可随时用到的对象
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
