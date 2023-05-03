using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


/// <summary>
/// 状态条
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
    
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 0.1f;
    
    /// <summary>
    /// 当前填充值
    /// </summary>
    private float _currentFillAmount; 
    /// <summary>
    /// 目标填充值
    /// </summary>
    protected float targetFillAmount; 
    /// <summary>
    /// 上次的填充值
    /// </summary>
    private float _previousFillAmount; 
    private float _t;
   
    WaitForSeconds waitForDelayFill;
    
    Coroutine bufferedFillingCoroutine;
    
    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
    
            canvas.worldCamera = Camera.main;
        }
           
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue,float maxValue)
    {
      
        _currentFillAmount = currentValue / maxValue;
        
        targetFillAmount = _currentFillAmount;
       
        fillImageBack.fillAmount = _currentFillAmount;
        fillImageFront.fillAmount = _currentFillAmount;  
    }

    
    public void UpdateStats(float currentValue,float maxValue)
    {
       
        targetFillAmount = currentValue/ maxValue;
        
        if(bufferedFillingCoroutine!=null)
            StopCoroutine(bufferedFillingCoroutine);
       
        if (_currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
           
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        
        else if(_currentFillAmount < targetFillAmount)
        {
           
            fillImageBack.fillAmount = targetFillAmount;
           
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }
    
    /// <summary>
    /// 缓充填冲协程
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        
        if (delayFill) yield return waitForDelayFill;
        
        _previousFillAmount = _currentFillAmount;
        
        _t = 0;
        while (_t < 1f)
        {
           
            _t += Time.deltaTime * fillSpeed;
            
            _currentFillAmount = Mathf.Lerp(_previousFillAmount, targetFillAmount, _t);
            
            image.fillAmount = _currentFillAmount;
           
            yield return null;
        }
    }
}
