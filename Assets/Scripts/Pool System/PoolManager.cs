using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager: MonoBehaviour
{
    //���˶��������
    [SerializeField] private Pool[] enemyPools;
    //����ӵ�������
    [SerializeField] private Pool[] playerProjectilePools;
    //�����ӵ�������
    [SerializeField] private Pool[] enemyProjectilePools;
    //�Ӿ���Ч������
    [SerializeField] private Pool[] vFXPools;
    //ս��Ʒ������
    [SerializeField] private Pool[] lootItemPools;
    //����Ԥ����������
    static Dictionary<GameObject, Pool> dictionary;
    void Awake()
    {
        //��ʼ���ֵ�
        dictionary = new Dictionary<GameObject, Pool>();
        
        //��ʼ�����˳ض���
        Initialize(enemyPools);
        
        //��ʼ������ӵ��ض���
        Initialize(playerProjectilePools);
        //��ʼ���л��ӵ��ض���
        Initialize(enemyProjectilePools);
        //��ʼ����Ч��
        Initialize(vFXPools);
        //��ʼ��ս��Ʒ���߳�
        Initialize(lootItemPools);
    }
    #if UNITY_EDITOR
    //�༭��ֹͣ����ʱ�Զ�����
    void OnDestroy()
    {
        //�����˶���سߴ�
        CheckPoolSize(enemyPools);
        
        //�������ӵ��سߴ�
        CheckPoolSize(playerProjectilePools);
        //���л��ӵ��سߴ�
        CheckPoolSize(enemyProjectilePools);
        //�����Ч�سߴ�
        CheckPoolSize(vFXPools);
        //���ս��Ʒ���߳�
        CheckPoolSize(lootItemPools);
        //Debug.Log("�Ѿ�����");
    }
    #endif
    
    //�����������ʱ�ĳߴ�
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            //��������ʵ�����гߴ���ڶ���سߴ� 
            if (pool.RuntimeSize > pool.Size)
            {
                //����̨��ӡ����
                Debug.LogWarning(
                    string.Format("Pool:{0} has a runtime size{1} bigger than its inital size{2}!",
                        pool.Prefab.name,
                        pool.RuntimeSize,
                        pool.Size));
            }
        }
    }
    //��ʼ�����ж����
    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            //��������ָ�ֻ��Unity�༭��������
        #if UNITY_EDITOR  
            //����ֵ�ļ�����һ����ͬԤ���壬ֱ���������ֵ��ÿ��������Ψһ
            if (dictionary.ContainsKey(pool.Prefab))
            {
                //����̨��ӡ������Ϣ
                Debug.LogError("Same prefab in multiple pools! Prefab:"+pool.Prefab.name);
                continue;
            }
        #endif
            //����ص�Ԥ���������ر�������ֵ�ԡ�ÿ��ʼ��һ������أ��ֵ�����һ��Ԥ����ͳصļ�ֵ��
            dictionary.Add(pool.Prefab, pool);
            //�½��ӵ��ĸ�����
            Transform poolParent =  new GameObject("Pool:" + pool.Prefab.name).transform;
            //�ӵ�������ĸ�����Ϊ�ع���������
            poolParent.parent = transform;
            //���ɶ�����е�������Ҫ�ĸ�����
            pool.Initialize(poolParent);
        }    
    }
    //��̬���������㱻���������
    /// <summary>
    /// ���ݴ�������ͷų���Ԥ����ɵĶ���
    /// </summary>
    /// <param name="prefab"></param> ָ���ͷŵĶ���
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR //�������룬ֻ�ڱ༭��������
        //�ֵ��в�����ָ����ֵʱ�����ؿ�ֵ
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could Not find prefab:" + prefab.name);
            return null;
        }
        #endif
        //���ݴ���Ԥ������ö�Ӧ������е�׼��������������һ������ʱ�õ��Ķ���
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
