using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

/// <summary>
/// GmmePlay UI控制器
/// </summary>
public class GameplayUIController : MonoBehaviour
{
    [Header("====== PLAYER INPUT ======")]
    [SerializeField] private PlayerInput playerInput;

    [Header("====== AUDIO DATA ======")]
    [SerializeField] private AudioData pauseSFX;
    [SerializeField] private AudioData unpauseSFX;
    
    [Header("====== CANVAS ======")]
    [SerializeField] private Canvas hUDCanvas;
    [SerializeField] private Canvas menusCanvas;

    [Header("====== PLAYER INPUT ======")] 
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    private int buttonPressedParameterID = Animator.StringToHash("Pressed");
    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;
    
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name,OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name,OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name,OnMainMenuButtonClick);
    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    /// <summary>
    /// 暂停输入事件处理器
    /// </summary>
    void Pause()
    {
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
    
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();

        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();

        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }
    
    /// <summary>
    /// 结束暂停输入事件处理器
    /// </summary>
    void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    /// <summary>
    /// 继承按钮事件处理器
    /// </summary>
    void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
    
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        
    
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }
    
    /// <summary>
    /// 选项按钮事件处理器
    /// </summary>
    void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    /// <summary>
    /// 主菜单按钮事件处理器
    /// </summary>
    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        //加载主场景
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
