using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")] 
    //���˵���������
    [SerializeField] private Canvas mainMenuCanvas;
    
    [Header("==== BUTTONS ====")] 
    // ��Ϸ��ť����
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonQuit;

    private void OnEnable()
    {
        buttonStart.onClick.AddListener(OnButtonStartClicked);
        //�Ǽǰ�ť���µ���Ϊ
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name,OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name,OnButtonOptionsClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name,OnButtonQuitClicked);
    } 
    void OnDisable()
    {
        buttonStart.onClick.RemoveAllListeners();
        //��ť������Ϊ�ű��İ�ť���ܱ��ֵ����
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        //ʱ��̶�����Ϊ1���������ͣ״̬�ص����˵���ʱ��̶�����Ϊ0
        Time.timeScale = 1f;
        
        //��Ϸ״̬Ϊ����
        GameManager.GameState = GameState.Playing;
        //���˵���ʱѡ�п�ʼ��ť
        UIInput.Instance.SelectUI(buttonStart);
    }

    //�����ʼ��ť�Ĺ��ܺ���
    void OnButtonStartClicked()
    {
        //�ر����˵�UI
        mainMenuCanvas.enabled = false;
        //����GamePlay����
        SceneLoader.Instance.LoadGamePlayScene();
    }
    //ѡ�ť����
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    //�˳���Ϸ��ť���ܺ���
    void OnButtonQuitClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
