using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    //���������ٶ�
    [SerializeField]Vector2 scrollVelocity;
    //���ڻ�ȡ�������
    Material material;
    void Awake()
    {
        //�õ��ı�����Ⱦ������Ĳ���
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        //��Ϸ״̬��Ϊ����״̬����ÿ֡������
        while (GameManager.GameState!=GameState.GameOver)
        {
            //�ı���ʵ��������Offsetֵ
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }

    // private void Update()
    // {
    //     //�ı���ʵ��������Offsetֵ
    //     material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    // }
    
}
