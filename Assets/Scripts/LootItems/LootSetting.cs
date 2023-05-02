using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��δ�̳�Mono�����е�Ԥ�л��ֶ���¶����
[System.Serializable] public class LootSetting
{
    //��Ϸս��Ʒ����
    public GameObject prefab;
    //ս��Ʒ���ʰٷֱ�
    [Range(0f, 100f)] public float dropPercentage;

    //����ս��Ʒ���߹��� ������ά�������������õ�������λ��
    public void Spawn(Vector3 position)
    {
        //�����ֵС�ڵ��ڵ���ֵ
        if (Random.Range(0f, 100f) <= dropPercentage)
        {
            //����ع���������һ��ս��Ʒ����
            PoolManager.Release(prefab, position);
        }
    }
}
