using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 分数UI控制器
/// </summary>
public class ScoringUIController : MonoBehaviour
{
    [Header("===== BACKGROUND ======")]
    /// <summary>
    /// 分数显示的背景图
    /// </summary>
    [SerializeField] private Image background;
    /// <summary>
    /// 分数背景的sprite组
    /// </summary>
    [SerializeField] private Sprite[] backgroundImages;

    [Header("===== SCORING SCREEN ======")] 
    /// <summary>
    /// 分数屏幕画布
    /// </summary>
    [SerializeField] private Canvas scoringScreenCanvas;
    /// <summary>
    /// 玩家分数显示的文本组件
    /// </summary>
    [SerializeField] private Text playerScoreText;
    
    [SerializeField] private Button buttonMainMenu;
    /// <summary>
    /// 高分排行榜
    /// </summary>
    [SerializeField] private Transform highScoreLeaderboardContainer;

    [Header("===== HIGH SCORE SCREEN ======")]
    /// <summary>
    /// 新高分屏幕画布
    /// </summary>
    [SerializeField] private Canvas newHighScoreScreenCanvas;
    [SerializeField] private Button buttonCancel;
    [SerializeField] private Button buttonSubmit;
    /// <summary>
    /// 玩家名称输入
    /// </summary>
    [SerializeField] private InputField playerNameInputField;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();

        if (ScoreManager.Instance.HasNewHighScore)
            ShowNewHighScoreScreen();
        else 
            ShowScoringScreen();
        
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name,OnButtonMainMenuClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.name,OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name,HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;
    }
    /// <summary>
    /// 显示新高分
    /// </summary>
    void ShowNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonCancel);
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }
    /// <summary>
    /// 随机显示背景
    /// </summary>
    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }
    
     /// <summary>
     /// 隐藏新高分屏幕
     /// </summary>
    void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
        
    }
    /// <summary>
    /// 显示分数画布
    /// </summary>
    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
       
        UpdateHighScoreLeaderboard();
    }

    /// <summary>
    /// 更新高分排行榜
    /// </summary>
    void UpdateHighScoreLeaderboard()
    {
        //获取玩家分数List
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            //将玩家分数数据逐一更新到排行榜的每行数据上
            var child = highScoreLeaderboardContainer.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }
    /// <summary>
    /// 主菜单按钮点击事件
    /// </summary>
    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    /// <summary>
    /// 确认按钮点击事件
    /// </summary>
    void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }
}
