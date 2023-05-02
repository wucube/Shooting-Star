using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIEventTrigger : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,ISelectHandler,ISubmitHandler
{
    //ѡ����Ч
    [SerializeField] private AudioData selectSFX;
    //�ύʱ����Ч����
    [SerializeField] private AudioData submitSFX;
    //ϵͳ��⵽�����ͣ�ڽű����ض����Ͼͻ���øú���
    public void OnPointerEnter(PointerEventData eventData)
    {
        //������Ƶ�������Ĳ�����Ч����
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    //��갴��ʱ���õĺ���
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
    //���ѡ��ʱ���õĺ���
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }
    //�����ǰʱ���õĺ���
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
