using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    void Awake()
    {
        //Ѱ����Ҷ�������
        //target = GameObject.FindGameObjectWithTag("Player");
        
        //������Ҷ���ΪĿ�����
        SetTarget(GameObject.FindWithTag("Player"));
    }
    protected override void OnEnable()
    {
        //����Э�̻�ȡ��ȷ���ƶ�����
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
        
    }
    // ʹ��Э�� ��ȡ��ȷ���ƶ����� ��������׼ȷ
    IEnumerator MoveDirectionCoroutine()
    {
        //����һ֡
        yield return null;
        
        //������û������������ִ��
        if (target.activeSelf)
        {
            //�޸��ӵ��ƶ�����Ŀ��λ��  - ��ǰλ�� �õ����ƶ����� ��һ���ƶ�����ȷ���ƶ��ٶȲ����
            moveDirection = (target.transform.position-transform.position).normalized;
        }
    }

}
