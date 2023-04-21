using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// 命中视效
    /// </summary>
    [SerializeField] GameObject hitVFX;

    /// <summary>
    /// 命中声效
    /// </summary>
    [SerializeField] AudioData[] hitSFX;

    /// <summary>
    /// 伤害
    /// </summary>
    [SerializeField] float damage;

    /// <summary>
    /// 子弹移动速度
    /// </summary>
    [SerializeField] protected float moveSpeed = 10f;

    /// <summary>
    /// 子弹移动方向
    /// </summary>
    [SerializeField] protected Vector2 moveDirection;

    /// <summary>
    /// 目标对象
    /// </summary>
    protected GameObject target;
    
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
    
    /// <summary>
    /// 子弹持续移动协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }
    
    /// <summary>
    /// 设置子弹命中目标
    /// </summary>
    /// <param name="target"></param>
    protected void SetTarget(GameObject target)=>this.target = target;
    
    /// <summary>
    /// 子弹移动
    /// </summary>
    public void Move() => transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Character character))
        {
            character.TakeDamage(damage);
            
            //对象管理器释放子弹命中视效，并设置位置与旋转角度
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            
            AudioManager.Instance.PlayerRandomSFX(hitSFX);
            
            //子弹对象失活，回收到对象池中
            gameObject.SetActive(false);
        }
    }
}
