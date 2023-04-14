using System.Collections;
using UnityEngine;

/// <summary>
/// 自动停用
/// </summary>
public class AutoDeactivate : MonoBehaviour
{
    /// <summary>
    /// 要销毁的对象
    /// </summary>
    [SerializeField] bool destroyGameObject;
    
    /// <summary>
    /// 销毁时间
    /// </summary>
    [SerializeField] float lifetime = 3f;
   
    /// <summary>
    /// 等待销毁的时间
    /// </summary>
    WaitForSeconds waitLifetime;

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }
    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }
    
    /// <summary>
    /// 停用协程
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        if (destroyGameObject)
            Destroy(gameObject);
        else 
            gameObject.SetActive(false);
    }
}