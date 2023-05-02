using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    //是否摧毁游戏对象
    [SerializeField] bool destroyGameObject;
    //生命周期时间
    [SerializeField] float lifetime = 3f;
    //等待生命周期
    WaitForSeconds waitLifetime;

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }
    void OnEnable()
    {
        //调用子弹禁用协程
        StartCoroutine(DeactivateCoroutine());
    }

    //子弹禁用协程
    IEnumerator DeactivateCoroutine()
    {
        //挂起等待对象生命周期
        yield return waitLifetime;
        //如果要摧毁
        if (destroyGameObject)
        {
            //销毁子弹对象
            Destroy(gameObject);
        }
        //不摧毁
        else 
        {
            //禁用子弹对象
            gameObject.SetActive(false);
        }
    }
}