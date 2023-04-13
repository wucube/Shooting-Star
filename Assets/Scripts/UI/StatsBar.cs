using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 状态条基类
/// </summary>
public class StatsBar : MonoBehaviour
{
    /// <summary>
    /// 背景填充图
    /// </summary>
    [SerializeField] Image fillImageBack;

    /// <summary>
    /// 前景填充图
    /// </summary>
    [SerializeField] Image fillImageFront;

    /// <summary>
    /// 是否延迟填充
    /// </summary>
    [SerializeField] bool delayFill = true;

    /// <summary>
    /// 延迟填充的时间
    /// </summary>
    [SerializeField] float fillDelay = 0.5f;
    
    /// <summary>
    /// 填充速度
    /// </summary>
    [SerializeField] float fillSpeed = 0.1f;
    
    /// <summary>
    /// 当前填充值
    /// </summary>
    private float currentFillAmount; 

    /// <summary>
    /// 目标填充值
    /// </summary>
    protected float targetFillAmount; 

    /// <summary>
    /// 先前的填充值
    /// </summary>
    private float previousFillAmount; 

    /// <summary>
    /// 延迟填充过程的时间
    /// </summary>
    private float t;
    /// <summary>
    /// 等待延迟填充
    /// </summary>
    WaitForSeconds waitForDelayFill;

    /// <summary>
    /// 延迟填充协程
    /// </summary>
    Coroutine bufferedFillingCoroutine;
    
    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            //头顶血条的填充，画布摄像使用世界空间模式
            canvas.worldCamera = Camera.main;
        }

        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    //对象死亡时停止填充协程
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// 初始化状态条
    /// </summary>
    /// <param name="currentValue">对象状态的当前值</param>
    /// <param name="maxValue">对象状态的最大值</param>
    public virtual void Initialize(float currentValue,float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;  
    }

    /// <summary>
    /// 更新状态条
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public void UpdateStats(float currentValue,float maxValue)
    {
        targetFillAmount = currentValue/ maxValue;

        //如果缓冲填充协程不为空，就先停掉延迟填充协程
        if(bufferedFillingCoroutine!=null)
            StopCoroutine(bufferedFillingCoroutine);

        //如果当前填充值 大于 目标填充值：状态条的值减少
        if (currentFillAmount > targetFillAmount)
        {
            //状态条前景图的填充值 立即达到目标值
            fillImageFront.fillAmount = targetFillAmount;

            //启用并记录延迟填充状态条背景图的协程
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        //如果当前填充值 小于 目标填充值：状态条的值增加
        else if(currentFillAmount < targetFillAmount)
        {
            //状态条背景图填充值立即到达目标值
            fillImageBack.fillAmount = targetFillAmount;

            //启用并记录延迟填充前景图的协程
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }
    
    /// <summary>
    /// 延迟填充协程
    /// </summary>
    /// <param name="image">要延迟填充的图片</param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        //若开启延迟填充
        if (delayFill) 
            yield return waitForDelayFill; //等待一定时间后开始填充

        //记录当前的填充值
        previousFillAmount = currentFillAmount;
        
        t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;

            //当前填充值不断向目标值变化
            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            //当前值赋值给图片填充值，实现状态条的变化
            image.fillAmount = currentFillAmount;
            
            yield return null;
        }
    }
}
