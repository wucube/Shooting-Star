using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主菜单按钮控制器
/// </summary>
public class MainMenuUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")] 
    [SerializeField] private Canvas mainMenuCanvas;
    
    [Header("==== BUTTONS ====")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonQuit;

    private void OnEnable()
    {
        buttonStart.onClick.AddListener(OnButtonStartClicked);

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name,OnButtonStartClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name,OnButtonOptionsClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name,OnButtonQuitClicked);
    } 
    void OnDisable()
    {
        buttonStart.onClick.RemoveAllListeners();
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    /// <summary>
    /// 开始按钮的点击事件处理器
    /// </summary>
    void OnButtonStartClicked()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }
    /// <summary>
    /// 选项按钮的点击事件处理器
    /// </summary>
    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }
    /// <summary>
    /// 退出的按钮点击事件
    /// </summary>
    void OnButtonQuitClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
