using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LootItem : MonoBehaviour
{
    //�����ƶ��ٶȱ���
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    
    //Ĭ��ʰȡ��Ч
    [SerializeField] protected AudioData defaultPickUpSFX;
    //����ʰȡ����������������תΪ��ϣֵ
    private int pickUpStateID = Animator.StringToHash("PickUp");
    //�����ϵĶ��������
    private Animator animator;
    //ʰȡ��Ч����
    protected AudioData pickUpSFX;
    //��Ҷ������
    protected Player player;
    
    //ʰȡ���ߵ��ı���Ϣ
    protected Text lootMessage;

    private void Awake()
    {
        //��ʼ���������������
        animator = GetComponent<Animator>();
        //��ʼ����Ҷ������
        player = FindObjectOfType<Player>();
        //��ʼ���ı���Ϣ������True����ȡ���õ����
        lootMessage = GetComponentInChildren<Text>(true);
        //��ʼ����ȡ��Ч����������Ĭ����Ч��ֵ
        pickUpSFX = defaultPickUpSFX;
    }

    private void OnEnable()
    {
        //���õ����ƶ�Э��
        StartCoroutine(MoveCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        //����ֹͣ�ƶ�
        StopAllCoroutines();
        //����ʰȡ����
        animator.Play(pickUpStateID);
        //����ʰȡ��Ч
        AudioManager.Instance.PlayerRandomSFX(pickUpSFX);
    }

    IEnumerator MoveCoroutine()
    {
        //���ȡ���ƶ��ٶ�
        float speed = Random.Range(minSpeed, maxSpeed);
        //�ƶ�����Ĭ���������
        Vector3 direction = Vector3.left;
        while (true)
        {
            //�����Ҵ��
            if (player.isActiveAndEnabled)
            {
                //�޸��ƶ���������
                direction = (player.transform.position - transform.position).normalized;
            }
            //��������ҷ����ƶ�
            transform.Translate(direction*speed*Time.deltaTime);
            yield return null;
        }
    }
}
