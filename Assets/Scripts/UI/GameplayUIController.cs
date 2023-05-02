using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class GameplayUIController : MonoBehaviour
{
    [Header("====== PLAYER INPUT ======")]
    //���������
    [SerializeField] private PlayerInput playerInput;

    [Header("====== AUDIO DATA ======")]
    //��ͣ��Ч
    [SerializeField] private AudioData pauseSFX;
    //ȡ����ͣ��Ч
    [SerializeField] private AudioData unpauseSFX;
    
    [Header("====== CANVAS ======")]
    //HUD����
    [SerializeField] private Canvas hUDCanvas;
    //�˵�����
    [SerializeField] private Canvas menusCanvas;

    [Header("====== PLAYER INPUT ======")] 
    //��ͣ�˵���������ť�������
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    //���������Ĺ�ϣֵ
    private int buttonPressedParameterID = Animator.StringToHash("Pressed");
    void OnEnable()
    {
        //��������������ͣ��ָ��¼�
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;
    
        //Ϊ����״̬����Ϊ�ű��еİ�ť���ܱ��ֵ���Ӽ�ֵ��
        //���������ť������ض�������ƣ�ֵ���������ť��Ӧ�Ĺ��ܺ��� 
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name,OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name,OnMainMenuButtonClick);
    }

    void OnDisable()
    {
        //�˶�����������ͣ��ָ��¼�
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
        //��հ�ť�������ܱ��еļ�ֵ��
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    //��ͣ����
    void Pause()
    {
        //�ر�HUD����
        hUDCanvas.enabled = false;
        //������ͣ�˵�����
        menusCanvas.enabled = true;
        
        //��Ϸ״̬Ϊ��ͣ
        GameManager.GameState = GameState.Paused;
        //����ʱ�����������ͣ����
        TimeController.Instance.Pause();
        
        //�л�����ͣ�˵�������
        playerInput.EnablePauseMenuInput();
        //����ģʽ�л�����̬����ģʽ
        playerInput.SwitchToDynamicUpdateMode();
        //����ѡ��UI����,��ͣ�˵��򿪾ͻ�ѡ�лָ���ť
        UIInput.Instance.SelectUI(resumeButton);
        //������ͣ��Ч
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    
    //ȡ����ͣ����
    void Unpause()
    {
        //��ѡ�лָ���ť
        resumeButton.Select();
        //����ť״̬�л�������״̬
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        //����ȡ����ͣ��Ч
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    //����ָ���ť�Ĺ��ܺ���
    void OnResumeButtonClick()
    {
        //ʱ��̶Ȼָ�����
        Time.timeScale = 1f;
        //����HUD����
        hUDCanvas.enabled = true;
        //������ͣ�˵�����
        menusCanvas.enabled = false;
        
        //��Ϸ״̬Ϊ����
        GameManager.GameState = GameState.Playing;
        //����ʱ��������ָ�����
        TimeController.Instance.Unpause();
        
        //�л���GamePlay������
        playerInput.EnableGameplayInput();
        //�л����̶�����ģʽ
        playerInput.SwitchToFixedUpdateMode();
    }
    
    //���ѡ�ť�Ĺ��ܺ���
    void OnOptionsButtonClick()
    {
        //ѡ�� ѡ��ܰ�ť 
        UIInput.Instance.SelectUI(optionsButton);
        //������ͣ�˵�������
        playerInput.EnablePauseMenuInput();
    }

    //������˵���ť�Ĺ��ܺ���
    void OnMainMenuButtonClick()
    {
        //��ͣ�˵��ر�
        menusCanvas.enabled = false;
        //�������˵�����
        SceneLoader.Instance.LoadMainMenuScene();
        // Load Main Menu Scene
    }
}
