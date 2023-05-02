using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    //�Ƿ�ݻ���Ϸ����
    [SerializeField] bool destroyGameObject;
    //��������ʱ��
    [SerializeField] float lifetime = 3f;
    //�ȴ���������
    WaitForSeconds waitLifetime;

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }
    void OnEnable()
    {
        //�����ӵ�����Э��
        StartCoroutine(DeactivateCoroutine());
    }

    //�ӵ�����Э��
    IEnumerator DeactivateCoroutine()
    {
        //����ȴ�������������
        yield return waitLifetime;
        //���Ҫ�ݻ�
        if (destroyGameObject)
        {
            //�����ӵ�����
            Destroy(gameObject);
        }
        //���ݻ�
        else 
        {
            //�����ӵ�����
            gameObject.SetActive(false);
        }
    }
}