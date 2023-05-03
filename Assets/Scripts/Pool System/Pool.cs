using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
[System.Serializable] 
public class Pool
{
    //用于获取游戏对象预制体的属性信息，游戏对象预制体变量是私有
    public GameObject Prefab => prefab;
    //用于获取游戏对象预制体
    [SerializeField] GameObject prefab;
    //对象池大小，即集合的长度
    [SerializeField] int size = 1;
    //对象池尺寸属性
    public int Size => size;
    //对象池运行时实际尺寸
    public int RuntimeSize=>queue.Count;
    
    //游戏对象数据集合队列
    Queue<GameObject> queue;
    //子弹制作体的父对象
    private Transform parent;
    //初始化对象集合
    public void Initialize(Transform parent)
    {
        //初始化队列
        queue = new Queue<GameObject>();
        //预制体父对象的位置
        this.parent = parent;
        for (int i = 0; i < size; i++)
        {
            //队列末尾添加预制体克隆
            queue.Enqueue(Copy());
        }
    }
    /// <summary>
    /// 克隆失活的预制体
    /// </summary>
    /// <returns></returns>
    GameObject Copy()
    {
        //生成预制体对象
        var copy = GameObject.Instantiate(prefab, parent);
        //禁用并返回
        copy.SetActive(false);
        return copy;
    }
    /// <summary>
    /// 取出可用对象
    /// </summary>
    /// <returns></returns>
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        //队列中有元素可用 队列第一个元素不是在启用状态中
        //Peek()函数返回队列的第一个元素，但不会将元素从队列中移除
        if(queue.Count > 0 && !queue.Peek().activeSelf)
            //取出队列第一个元素，并从队列中移除
            availableObject = queue.Dequeue();
        //队列中无元素可用
        else
        //生成新的对象
            availableObject = Copy();
        
        //取出对象后立即入列
        queue.Enqueue(availableObject);
        return availableObject;  
    }

    //预备完成的对象
    public GameObject PreparedObject()
    {
        //存储取出的可用对象
        GameObject  preparedObject = AvailableObject();
        //启用可用对象
        preparedObject.SetActive(true);
        return preparedObject;
    }
    //预备完成的对象，指定对象位置 
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        return preparedObject;
    }
    //预备完成对象，指定对象的位置与旋转角度
    public GameObject PreparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }
    //预备完成对象，指定对象的位置与旋转角度和绽放大小
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