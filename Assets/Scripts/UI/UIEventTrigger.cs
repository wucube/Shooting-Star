using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI事件触发
/// </summary>
public class UIEventTrigger : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,ISelectHandler,ISubmitHandler
{
    /// <summary>
    /// UI选择时的音效
    /// </summary>
    [SerializeField] private AudioData selectSFX;
    /// <summary>
    /// UI提交时的音效
    /// </summary>
    [SerializeField] private AudioData submitSFX;
    
    /// <summary>
    /// 鼠标进入按钮时触发
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    /// <summary>
    /// 鼠标按下按钮时触发
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
    /// <summary>
    /// 鼠标选中按钮时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    /// <summary>
    /// 鼠标提交按钮时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
