using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI事件触发器
/// </summary>
public class UIEventTrigger : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,ISelectHandler,ISubmitHandler
{
    //选中音效
    [SerializeField] private AudioData selectSFX;
    //提交时的音效变量
    [SerializeField] private AudioData submitSFX;
    //系统检测到鼠标悬停在脚本挂载对象上就会调用该函数
    public void OnPointerEnter(PointerEventData eventData)
    {
        //调用音频管理器的播放音效函数
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    //鼠标按下时调用的函数
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
    //鼠标选中时调用的函数
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    //鼠标提前时调用的函数
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
