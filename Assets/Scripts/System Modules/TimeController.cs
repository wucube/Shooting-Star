using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 时间控制器
/// </summary>
public class TimeController : Singleton<TimeController>
{
    //进入子弹时间的时间刻度值
    [SerializeField, Range(0f, 1f)] private float bulletTimeScale = 0.1f;
    //存储默认固定帧时间的值
    private float _defaultFixedDeltaTime;
    //游戏暂停前的时间刻度
    private float _timeScaleBeforePause;
    //线性插值第三参数 t
    private float _t;
    protected override void Awake()
    {
        base.Awake();
        //赋予默认固定帧时间的值
        _defaultFixedDeltaTime = Time.fixedDeltaTime;
    }
    //暂停函数
    public void Pause()
    {
        //先记录当前帧的时间刻度
        _timeScaleBeforePause = Time.timeScale;
        //时间刻度为0
        Time.timeScale = 0f;
    }
    //取消暂停函数
    public void Unpause()
    {
        //按下暂停键时的时间刻度值赋予当前时间刻度
        Time.timeScale = _timeScaleBeforePause;
    }
    //子弹时间协程
    public void BulletTime(float duration)
    {
        //修改时间刻度
        Time.timeScale = bulletTimeScale;
        //启用时间刻度恢复协程
        StartCoroutine(SlowOutCoroutine(duration));
    }
    //子弹时间函数，时间先变慢，再恢复
    public void BulletTime(float inDuration, float outDuration)
    {
        //调用时间刻度先减少再恢复的协程
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }
    //子弹时间函数，时间变慢后持续一段时间再恢复
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        //调用时间刻度减少后持续一段时间再恢复的协程
        StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }
    //时间变慢函数 
    public void SlowIn(float duration)
    {
        //调用时间变慢协程
        StartCoroutine(SlowInCoroutine(duration));
    }
    //时间恢复函数
    public void SlowOut(float duration)
    {
        //调用时间恢复协程
        StartCoroutine(SlowOutCoroutine(duration));
    }
    //时间变慢后持续一段时间再恢复 协程
    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        //先挂起执行时间刻度减少协程
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        //挂起等待一段时间 WaitForSecondsRealtime，不受时间刻度影响
        yield return new WaitForSecondsRealtime(keepingDuration);
        //再执行时间刻度恢复协程
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    //缓慢减少时间刻度再缓慢增加时间刻度的协程
    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        //先挂起执行时间刻度减少协程
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        //再执行时间刻度恢复协程
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    //缓慢恢复时间刻度协程 浮点型参数存储过程持续时间
    IEnumerator SlowOutCoroutine(float duration)
    {
        _t = 0f;
        while (_t<1f)
        {
            //如果游戏状态为非暂停状态，则修改时间刻度值
            if (GameManager.GameState!=GameState.Paused)
            {
                //Time.unscaledDeltaTime 不受时间刻度值影响的帧间值
                _t += Time.unscaledDeltaTime / duration;
                //修改时间刻度
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, _t);
                //修改固定帧时间，默认固定帧时间值 乘 修改后的时间刻度值
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;
            }
            //挂起直到下一帧继续循环
            yield return null;
        }
    }
    //缓慢减少时间刻度协程
    IEnumerator SlowInCoroutine(float duration)
    {
        _t = 0f;
        while (_t<1f)
        {
            if (GameManager.GameState!=GameState.Paused)
            {
                //Time.unscaledDeltaTime 不受时间刻度值影响的帧间值
                _t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, _t);
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
}
