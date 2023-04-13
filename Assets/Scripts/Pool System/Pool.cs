using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //��δ�̳�Mono�����е�Ԥ�л��ֶ���¶����
public class Pool
{
    //���ڻ�ȡ��Ϸ����Ԥ�����������Ϣ����Ϸ����Ԥ���������˽��
    public GameObject Prefab => prefab;
    //���ڻ�ȡ��Ϸ����Ԥ����
    [SerializeField] GameObject prefab;
    //����ش�С�������ϵĳ���
    [SerializeField] int size = 1;
    //����سߴ�����
    public int Size => size;
    //���������ʱʵ�ʳߴ�
    public int RuntimeSize=>queue.Count;
    
    //��Ϸ�������ݼ��϶���
    Queue<GameObject> queue;
    //�ӵ�������ĸ�����
    private Transform parent;
    //��ʼ�����󼯺�
    public void Initialize(Transform parent)
    {
        //��ʼ������
        queue = new Queue<GameObject>();
        //Ԥ���常�����λ��
        this.parent = parent;
        for (int i = 0; i < size; i++)
        {
            //����ĩβ���Ԥ�����¡
            queue.Enqueue(Copy());
        }
    }
    //����Ԥ����
    GameObject Copy()
    {
        //����Ԥ�������
        var copy = GameObject.Instantiate(prefab, parent);
        //���ò�����
        copy.SetActive(false);
        return copy;
    }
    //ȡ�����ö���
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        //��������Ԫ�ؿ��� ���е�һ��Ԫ�ز���������״̬��
        //Peek()�������ض��еĵ�һ��Ԫ�أ������ὫԪ�شӶ������Ƴ�
        if(queue.Count > 0 &&!queue.Peek().activeSelf)
            //ȡ�����е�һ��Ԫ�أ����Ӷ������Ƴ�
            availableObject = queue.Dequeue();
        //��������Ԫ�ؿ���
        else
        //�����µĶ���
            availableObject = Copy();
        
        //ȡ���������������
        queue.Enqueue(availableObject);
        return availableObject;  
    }

    //Ԥ����ɵĶ���
    public GameObject PreparedObject()
    {
        //�洢ȡ���Ŀ��ö���
        GameObject  preparedObject = AvailableObject();
        //���ÿ��ö���
        preparedObject.SetActive(true);
        return preparedObject;
    }
    //Ԥ����ɵĶ���ָ������λ�� 
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        return preparedObject;
    }
    //Ԥ����ɶ���ָ�������λ������ת�Ƕ�
    public GameObject PreparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }
    //Ԥ����ɶ���ָ�������λ������ת�ǶȺ����Ŵ�С
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
