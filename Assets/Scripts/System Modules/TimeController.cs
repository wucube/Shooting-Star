using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    //�����ӵ�ʱ���ʱ��̶�ֵ
    [SerializeField, Range(0f, 1f)] private float bulletTimeScale = 0.1f;
    //�洢Ĭ�Ϲ̶�֡ʱ���ֵ
    private float _defaultFixedDeltaTime;
    //��Ϸ��ͣǰ��ʱ��̶�
    private float _timeScaleBeforePause;
    //���Բ�ֵ�������� t
    private float _t;
    protected override void Awake()
    {
        base.Awake();
        //����Ĭ�Ϲ̶�֡ʱ���ֵ
        _defaultFixedDeltaTime = Time.fixedDeltaTime;
    }
    //��ͣ����
    public void Pause()
    {
        //�ȼ�¼��ǰ֡��ʱ��̶�
        _timeScaleBeforePause = Time.timeScale;
        //ʱ��̶�Ϊ0
        Time.timeScale = 0f;
    }
    //ȡ����ͣ����
    public void Unpause()
    {
        //������ͣ��ʱ��ʱ��̶�ֵ���赱ǰʱ��̶�
        Time.timeScale = _timeScaleBeforePause;
    }
    //�ӵ�ʱ��Э��
    public void BulletTime(float duration)
    {
        //�޸�ʱ��̶�
        Time.timeScale = bulletTimeScale;
        //����ʱ��̶Ȼָ�Э��
        StartCoroutine(SlowOutCoroutine(duration));
    }
    //�ӵ�ʱ�亯����ʱ���ȱ������ٻָ�
    public void BulletTime(float inDuration, float outDuration)
    {
        //����ʱ��̶��ȼ����ٻָ���Э��
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }
    //�ӵ�ʱ�亯����ʱ����������һ��ʱ���ٻָ�
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        //����ʱ��̶ȼ��ٺ����һ��ʱ���ٻָ���Э��
        StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));
    }
    //ʱ��������� 
    public void SlowIn(float duration)
    {
        //����ʱ�����Э��
        StartCoroutine(SlowInCoroutine(duration));
    }
    //ʱ��ָ�����
    public void SlowOut(float duration)
    {
        //����ʱ��ָ�Э��
        StartCoroutine(SlowOutCoroutine(duration));
    }
    //ʱ����������һ��ʱ���ٻָ� Э��
    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        //�ȹ���ִ��ʱ��̶ȼ���Э��
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        //����ȴ�һ��ʱ�� WaitForSecondsRealtime������ʱ��̶�Ӱ��
        yield return new WaitForSecondsRealtime(keepingDuration);
        //��ִ��ʱ��̶Ȼָ�Э��
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    //��������ʱ��̶��ٻ�������ʱ��̶ȵ�Э��
    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        //�ȹ���ִ��ʱ��̶ȼ���Э��
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        //��ִ��ʱ��̶Ȼָ�Э��
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    //�����ָ�ʱ��̶�Э�� �����Ͳ����洢���̳���ʱ��
    IEnumerator SlowOutCoroutine(float duration)
    {
        _t = 0f;
        while (_t<1f)
        {
            //�����Ϸ״̬Ϊ����ͣ״̬�����޸�ʱ��̶�ֵ
            if (GameManager.GameState!=GameState.Paused)
            {
                //Time.unscaledDeltaTime ����ʱ��̶�ֵӰ���֡��ֵ
                _t += Time.unscaledDeltaTime / duration;
                //�޸�ʱ��̶�
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, _t);
                //�޸Ĺ̶�֡ʱ�䣬Ĭ�Ϲ̶�֡ʱ��ֵ �� �޸ĺ��ʱ��̶�ֵ
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;
            }
            //����ֱ����һ֡����ѭ��
            yield return null;
        }
    }
    //��������ʱ��̶�Э��
    IEnumerator SlowInCoroutine(float duration)
    {
        _t = 0f;
        while (_t<1f)
        {
            if (GameManager.GameState!=GameState.Paused)
            {
                //Time.unscaledDeltaTime ����ʱ��̶�ֵӰ���֡��ֵ
                _t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, _t);
                Time.fixedDeltaTime = _defaultFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }
}
