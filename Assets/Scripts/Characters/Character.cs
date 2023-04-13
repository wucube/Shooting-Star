using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 角色基类
/// </summary>
public class Character : MonoBehaviour
{
    [Header("------ DEATH -------")]

    [SerializeField] GameObject deathVFX;
    /// <summary>
    /// 死亡音效数组
    /// </summary>
    [SerializeField] AudioData[] deathSFX;

    [Header("------ HEALTH -------")]
    /// <summary>
    /// 最大血量值
    /// </summary>
    [SerializeField] protected float maxHealth;

    /// <summary>
    /// 头顶血条
    /// </summary>
    [SerializeField] StatsBar onHeadHealthBar;

    /// <summary>
    /// 是否显示头顶血条
    /// </summary>
    [SerializeField] private bool showOnHeadHealthBar = true;

    /// <summary>
    /// 当前血量值
    /// </summary>
    protected float health;

    protected virtual void OnEnable()
    {
        health = maxHealth;
 
        if (showOnHeadHealthBar) ShowOnHeadHealthBar();

        else HideOnHeadHealthBar();
    }

    /// <summary>
    /// 显示头顶血条
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);

        onHeadHealthBar.Initialize(health, maxHealth);
    }

    /// <summary>
    /// 隐藏头顶血条
    /// </summary>
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        //血量值为0直接返回出去
        if(health==0f) return;

        //当前血量值扣除伤害值
        health -= damage;

        //头顶血条显示就更新血条
        if (showOnHeadHealthBar)
            onHeadHealthBar.UpdateStats(health, maxHealth);

        //血量值小等于0，调用死亡函数
        if (health <= 0) Die();
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public virtual void Die()
    {
        //当前血量值为0
        health = 0f;
        //播放死亡音效
        AudioManager.Instance.PlayerRandomSFX(deathSFX);
        //对象池释放死亡特效
        PoolManager.Release(deathVFX, transform.position);
        //角色对象失活
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 恢复血量
    /// </summary>
    /// <param name="value"></param>
    public virtual void RestoreHealth(float value)
    {
        //当前血量值等于最大血量值 直接返回出去
        if (health == maxHealth) return;

        //当前血量总是在 0~最大血量之间
        health = Mathf.Clamp(health + value, 0f, maxHealth);

        //头顶血量显示，则更新血条显示
        if (showOnHeadHealthBar)
            onHeadHealthBar.UpdateStats(health, maxHealth);
    }
    
    /// <summary>
    /// 血量逐渐再生协程
    /// </summary>
    /// <param name="waitTime">血量延迟恢复的时间</param>
    /// <param name="percent">血量恢复的百分比</param>
    /// <returns></returns>
    public IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        //当前血量大小最大血量，就一直恢复血量
        while (health < maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    } 

    /// <summary>
    /// 持续伤害协程
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while(health>0f)
        {

            yield return waitTime;

            TakeDamage(maxHealth * percent);
        }
    }
}
