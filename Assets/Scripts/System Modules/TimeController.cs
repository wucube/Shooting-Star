using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 时间控制器
/// </summary>
public class TimeController : Singleton<TimeController>
{
    /// <summary>
    /// 子弹时间的时间刻度缩放值
    /// </summary>
    /// <returns></returns>
    [SerializeField, Range(0f, 1f)] private float bulletTimeScale = 0.1f;
    
    /// <summary>
    /// 默认帧间隔时间值
    /// </summary>
    private float defaultFixedDeltaTime;
    /// <summary>
    /// 暂停之前的时间缩放值
    /// </summary>
    private float timeScaleBeforePause;
    private float t;
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 恢复暂停
    /// </summary>
    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
    }

    /// <summary>
    /// 子弹时间
    /// </summary>
    /// <param name="duration">慢出的时间</param>
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }
    /// <summary>
    /// 子弹时间
    /// </summary>
    /// <param name="inDuration">进入的时间</param>
    /// <param name="outDuration">退出的时间</param>
    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }
    /// <summary>
    /// 子弹时间
    /// </summary>
    /// <param name="inDuration">进入的时间</param>
    /// <param name="keepingDuration">持续的时间</param>
    /// <param name="outDuration">退出的时间</param>
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }
    
    /// <summary>
    /// 慢入
    /// </summary>
    /// <param name="duration"></param>
    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }
    /// <summary>
    /// 慢出
    /// </summary>
    /// <param name="duration"></param>
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }
    /// <summary>
    /// 慢入-持续-退出
    /// </summary>
    /// <param name="inDuration"></param>
    /// <param name="keepingDuration"></param>
    /// <param name="outDuration"></param>
    /// <returns></returns>
    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    /// <summary>
    /// 慢入-慢出的协程
    /// </summary>
    /// <param name="inDuration"></param>
    /// <param name="outDuration"></param>
    /// <returns></returns>
    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    /// <summary>
    /// 慢出协程
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0f;
        while (t<1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 慢入协程
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0f;
        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
}
