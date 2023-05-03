using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    //玩家输入类变量
    [SerializeField] private PlayerInput input;
    //HUD画布变量
    [SerializeField] private Canvas HUDCanvas;
    //确认游戏结束音效
    [SerializeField] private AudioData confirmGameOverSound;
    //游戏结束画面退出状态ID
    private int exitStateID = Animator.StringToHash("GameOverScreenExit");
    
    //游戏结束画面UI画布组件
    private Canvas _canvas;
    //动画器组件，控制动画的播放
    private Animator _animator;

    private void Awake()
    {
        //获取画布
        _canvas = GetComponent<Canvas>();
        //获取动画器
        _animator = GetComponent<Animator>();
        //禁用画布与动画器
        _canvas.enabled = false;
        _animator.enabled = false;
    }

    private void OnEnable()
    {
        //订阅游戏结束委托
        GameManager.onGameOver += OnGameOver;
        //订阅玩家按键事件
        input.onConfirmGameOver += OnConfirmGameOver;
    }

    private void OnDisable()
    {
        //退订游戏结束委托
        GameManager.onGameOver -= OnGameOver;
        //退订玩家按键事件
        input.onConfirmGameOver -= OnConfirmGameOver;
    }

    //确认游戏结束按键事件处理函数 
    private void OnConfirmGameOver()
    {
        //播放确认结束的效果音
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        //禁用玩家所有输入
        input.DisableAllInputs();
        //游戏画面结束的退出动画
        _animator.Play(exitStateID);
        //加载计分场景
        SceneLoader.Instance.LoadScoringScene(); // TODO
    }
    //游戏结束委托处理函数
    void OnGameOver()
    {
        //关闭HUD界面
        HUDCanvas.enabled = false;
        //开启游戏结束画面
        _canvas.enabled = true;
        //启用动画器组件播放动画
        _animator.enabled = true;
        //禁用玩家所有输入
        input.DisableAllInputs();
    }
    
    //启用游戏结束画面输入，动画事件
    void EnableGameOverScreenInput()
    {
        //切换到游戏结束动作表
        input.EnableGameOverScreenInput();
    }
}
