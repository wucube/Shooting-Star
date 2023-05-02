using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    //ս��Ʒ��������
    [SerializeField] private LootSetting[] lootSettings;

    //����ս��Ʒ���� ������ά��������ս��Ʒ����
    public void Spawn(Vector2 position)
    {
        //����ս��Ʒ��������
        foreach (var item in lootSettings)
        {
            //����������ÿ��ս��Ʒ���õ����ɺ������������ɲ���һ��ƫ��
            item.Spawn(position+Random.insideUnitCircle);
        }
    }
}
