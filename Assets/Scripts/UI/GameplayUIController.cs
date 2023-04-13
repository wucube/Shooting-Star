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
    //玩家输入类
    [SerializeField] private PlayerInput playerInput;

    [Header("====== AUDIO DATA ======")]
    //暂停音效
    [SerializeField] private AudioData pauseSFX;
    //取消暂停音效
    [SerializeField] private AudioData unpauseSFX;
    
    [Header("====== CANVAS ======")]
    //HUD画布
    [SerializeField] private Canvas hUDCanvas;
    //菜单画布
    [SerializeField] private Canvas menusCanvas;

    [Header("====== PLAYER INPUT ======")] 
    //暂停菜单的三个按钮组件变量
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    //函数参数的哈希值
    private int buttonPressedParameterID = Animator.StringToHash("Pressed");
    void OnEnable()
    {
        //订阅玩家输入的暂停与恢复事件
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;
    
        //为动画状态机行为脚本中的按钮功能表字典添加键值对
        //键传入各按钮组件挂载对象的名称，值传入各个按钮对应的功能函数 
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name,OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name,OnMainMenuButtonClick);
    }

    void OnDisable()
    {
        //退订玩家输入的暂停与恢复事件
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
        //清空按钮函数功能表中的键值对
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    //暂停函数
    void Pause()
    {
        //关闭HUD画布
        hUDCanvas.enabled = false;
        //开启暂停菜单画布
        menusCanvas.enabled = true;
        
        //游戏状态为暂停
        GameManager.GameState = GameState.Paused;
        //调用时间控制器的暂停函数
        TimeController.Instance.Pause();
        
        //切换到暂停菜单动作表
        playerInput.EnablePauseMenuInput();
        //输入模式切换到动态更新模式
        playerInput.SwitchToDynamicUpdateMode();
        //调用选中UI函数,暂停菜单打开就会选中恢复按钮
        UIInput.Instance.SelectUI(resumeButton);
        //播放暂停音效
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    
    //取消暂停函数
    void Unpause()
    {
        //先选中恢复按钮
        resumeButton.Select();
        //将按钮状态切换到按下状态
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        //播放取消暂停音效
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    //点击恢复按钮的功能函数
    void OnResumeButtonClick()
    {
        //时间刻度恢复正常
        Time.timeScale = 1f;
        //开启HUD界面
        hUDCanvas.enabled = true;
        //隐藏暂停菜单界面
        menusCanvas.enabled = false;
        
        //游戏状态为运行
        GameManager.GameState = GameState.Playing;
        //调用时间管理器恢复函数
        TimeController.Instance.Unpause();
        
        //切换到GamePlay动作表
        playerInput.EnableGameplayInput();
        //切换到固定更新模式
        playerInput.SwitchToFixedUpdateMode();
    }
    
    //点击选项按钮的功能函数
    void OnOptionsButtonClick()
    {
        //选中 选项功能按钮 
        UIInput.Instance.SelectUI(optionsButton);
        //开启暂停菜单动作表
        playerInput.EnablePauseMenuInput();
    }

    //点击主菜单按钮的功能函数
    void OnMainMenuButtonClick()
    {
        //暂停菜单关闭
        menusCanvas.enabled = false;
        //加载主菜单场景
        SceneLoader.Instance.LoadMainMenuScene();
        // Load Main Menu Scene
    }
}
