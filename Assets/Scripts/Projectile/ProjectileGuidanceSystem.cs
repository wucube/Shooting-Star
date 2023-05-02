using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    //�ӵ���ű����ñ���
    [SerializeField] private Projectile projectile;
    //��С�����Ƕ�
    [SerializeField] private float minBallisticAngle = -50f;
    //��󵯵��Ƕ�
    [SerializeField] private float maxBallisticAngle = 50f;
    //�ӵ������Ŀ��ĳ���
    private Vector3 _targetDirection;
    //�����Ƕȱ���
    private float _ballisticAngle;
    //�ӵ��鳲Э�̡��鳲������������ͨ����ָ�ӵ������Զ�׷�ٹ���
    public IEnumerator HomingCoroutine(GameObject target)
    {
        //ѭ����ʼǰ�������������Ϊ�����Ƕȸ�ֵ
        _ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        //���ӵ����ڻ״̬ʱ�������ѭ��
        while (gameObject.activeSelf)
        {
            //���Ŀ�괦�ڻ״̬���ӵ�������Ŀ���ƶ�
            if (target.activeSelf)
            {
                //Ŀ������ - �ӵ�����λ������ ���ӵ����Ŀ��ĳ�������
                _targetDirection = target.transform.position - transform.position;
                //ת���ӵ�Z�ᣬ���ӵ�ʼ�ճ���Ŀ��
                //�÷����к���ȡ�ó���Ŀ�����ת���ȣ�����X���� �����Ŀ������ֱ�ߵĻ���
                transform.rotation =
                    Quaternion.AngleAxis(Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg,
                        Vector3.forward);
                //�����ӵ�������ٽ��ӵ���ת����һ����Ԫ�Ƶ�ŷ���ǣ�ֻ��Z��Ƕ�  
                transform.rotation *= Quaternion.Euler(0f,0f,_ballisticAngle);
                //�����ӵ��ƶ����� 
                projectile.Move();
            }
            //���Ŀ�겻���ڻ״̬�������ӵ�����ԭ���趨�õ��ƶ�����ǰ��
            else projectile.Move();
            yield return null;
        }
    }
}
