using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")] 
    //主菜单画布变量
    [SerializeField] private Canvas mainMenuCanvas;
    
    [Header("==== BUTTONS ====")] 
    // 游戏按钮变量
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonQuit;

    private void OnEnable()
    {
        buttonStart.onClick.AddListener(OnButtonStartClicked);
        //登记按钮按下的行为
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name,OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name,OnButtonOptionsClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name,OnButtonQuitClicked);
    } 
    void OnDisable()
    {
        buttonStart.onClick.RemoveAllListeners();
        //按钮按下行为脚本的按钮功能表字典清空
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        //时间刻度设置为1。如果从暂停状态回到主菜单，时间刻度依旧为0
        Time.timeScale = 1f;
        
        //游戏状态为运行
        GameManager.GameState = GameState.Playing;
        //主菜单打开时选中开始按钮
        UIInput.Instance.SelectUI(buttonStart);
    }

    //点击开始按钮的功能函数
    void OnButtonStartClicked()
    {
        //关闭主菜单UI
        mainMenuCanvas.enabled = false;
        //加载GamePlay场景
        SceneLoader.Instance.LoadGamePlayScene();
    }
    //选项按钮函数
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    //退出游戏按钮功能函数
    void OnButtonQuitClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
