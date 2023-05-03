using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData[] hitSFX;
    [SerializeField] float damage;
   
    [SerializeField] protected float moveSpeed = 10f;
    
    [SerializeField] protected Vector2 moveDirection;
    
    protected GameObject target;
    
    protected virtual void OnEnable()
    {
       
        StartCoroutine(MoveDirectly());
    }
    
    /// <summary>
    /// 子弹直接移动协程
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
    /// 设置子弹跟踪目标
    /// </summary>
    /// <param name="target"></param>
    protected void SetTarget(GameObject target)=>this.target = target;
    
    public void Move()=>transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Character character))
        {
            character.TakeDamage(damage);
            
            PoolManager.Release(hitVFX, collision.GetContact(0).point,
                Quaternion.LookRotation(collision.GetContact(0).normal));
            
            AudioManager.Instance.PlayerRandomSFX(hitSFX);
            
            gameObject.SetActive(false);
        }
    }
}
