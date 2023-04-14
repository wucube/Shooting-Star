using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 池子类
/// </summary>
[System.Serializable] 
public class Pool
{
    public GameObject Prefab => prefab;
    [SerializeField] private GameObject prefab;

    /// <summary>
    /// 对象池初始容量
    /// </summary>
    [SerializeField] private int size = 1;

    /// <summary>
    /// 对象池初始容量
    /// </summary>
    public int Size => size;

    /// <summary>
    /// 对象池运行时的实际容量
    /// </summary>
    public int RuntimeSize => queue.Count;
    
    /// <summary>
    /// 存储对象的容器
    /// </summary>
    private Queue<GameObject> queue;
    
    /// <summary>
    /// 对象的父对象
    /// </summary>
    private Transform parent;
    
    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="parent">对象的父对象</param>
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();

        this.parent = parent;

        //根据对象池容量添加对象
        for (int i = 0; i < size; i++)
            queue.Enqueue(Copy());
        
    }

    /// <summary>
    /// 复制对象
    /// </summary>
    /// <returns>返回未激活的对象</returns>
    private GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);
        copy.SetActive(false);
        return copy;
    }
    
    /// <summary>
    /// 可用对象
    /// </summary>
    /// <returns>返回未激活的对象</returns>
    private GameObject AvailableObject()
    {
        GameObject availableObject = null;
        
        //若 对象容器中存在对象 且 容器的头部对象未激活
        if(queue.Count > 0 && !queue.Peek().activeSelf)
            //从容器中取出头部对象
            availableObject = queue.Dequeue();

        else //否则复制一个对象
            availableObject = Copy();
        
        //将可用对象再加入到容器尾部。采用队列作为对象容器，取出的可用对象立即加入到队尾，如此就可只判断 容器中对象数量和队首的对象是否可用 来判断整个对象容器中是否还有可用对象。
        queue.Enqueue(availableObject);

        return availableObject;  
    }

    /// <summary>
    /// 准备完成的对象
    /// </summary>
    /// <returns>返回激活的对象</returns>
    public GameObject PreparedObject()
    {
        GameObject  preparedObject = AvailableObject();
        //激活可用对象
        preparedObject.SetActive(true);

        return preparedObject;
    }

    /// <summary>
    /// 准备完成的对象
    /// </summary>
    /// <param name="position">对象的位置</param>
    /// <returns>返回激活的对象</returns>
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }
    /// <summary>
    /// 准备完成的对象
    /// </summary>
    /// <param name="position">对象的位置</param>
    /// <param name="rotation">对象的旋转</param>
    /// <returns>返回激活的对象</returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }
    /// <summary>
    /// 准备完成的对象
    /// </summary>
    /// <param name="position">对象的位置</param>
    /// <param name="rotation">对象的旋转</param>
    /// <param name="localScale">对象的缩放</param>
    /// <returns>返回激活的对象</returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
}
