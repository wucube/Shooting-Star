using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoringUIController : MonoBehaviour
{
    [Header("===== BACKGROUND ======")]
    //背景图片Image组件
    [SerializeField] private Image background;
    //所有要显示的图片数组
    [SerializeField] private Sprite[] backgroundImages;

    [Header("===== SCORING SCREEN ======")] 
    //积分画面画布组件
    [SerializeField] private Canvas scoringScreenCanvas;
    //玩家分数文本组件
    [SerializeField] private Text playerScoreText;
    //主菜单按钮组件
    [SerializeField] private Button buttonMainMenu;
    //高分排行榜容器
    [SerializeField] private Transform highScoreLeaderboardContainer;

    [Header("===== HIGH SCORE SCREEN ======")]
    //新高分UI画布组件
    [SerializeField] private Canvas newHighScoreScreenCanvas;
    //按钮变量
    [SerializeField] private Button buttonCancel;
    [SerializeField] private Button buttonSubmit;
    //文本输入框变量
    [SerializeField] private InputField playerNameInputField;

    private void Start()
    {
        //一进入计分场景，就会显示光标
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //调用随机显示背景图片函数
        ShowRandomBackground();
        //玩家是否取得了新高分
        if (ScoreManager.Instance.HasNewHighScore)
        {
            //显示新高分画面
            ShowNewHighScoreScreen();
        }
        else //没有取得新高分画面，显示得分UI
            ShowScoringScreen();
        //登记按钮功能函数
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name,OnButtonMainMenuClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.name,OnButtonSubmitClicked);
        //取消按钮功能就是关闭新高分画面
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name,HideNewHighScoreScreen);
        //游戏进入计分状态
        GameManager.GameState = GameState.Scoring;
    }
    //显示新高分画面
    void ShowNewHighScoreScreen()
    {
        //开启UI画布
        newHighScoreScreenCanvas.enabled = true;
        //选中取消按钮
        UIInput.Instance.SelectUI(buttonCancel);
    }

    private void OnDisable()
    {
        //清空按钮功能表字典
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    //显示随机背景图片
    void ShowRandomBackground()
    {
        //随机一张背景图片数组中的图片，赋值给背景图片组件的sprite
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
        
    }
    
    //关闭新分画面UI
    void HideNewHighScoreScreen()
    {
        //隐藏UI画面
        newHighScoreScreenCanvas.enabled = false;
        //保存玩家得分数据
        ScoreManager.Instance.SavePlayerScoreData();
        //显示随机背景
        ShowRandomBackground();
        //显示随机画面
        ShowScoringScreen();
        
    }
    //显示积分画面
    void ShowScoringScreen()
    {
        //开启计分画面UI
        scoringScreenCanvas.enabled = true;
        //显示玩家最终得分
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        //选中主菜单按钮
        UIInput.Instance.SelectUI(buttonMainMenu);
       
        //更新高分排行榜
        
        UpdateHighScoreLeaderboard();
    }

    //更新高分排行榜数据
    void UpdateHighScoreLeaderboard()
    {
        //读取并记录玩家得分数据列表
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;
        //将列表数据一一显示在UI上
        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            //位次取得每个UI子对象
            var child = highScoreLeaderboardContainer.GetChild(i);
            //UI子对象找到自己的文本子对象，玩家数据赋值给文本组件
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }
    //主菜单按钮功能函数
    void OnButtonMainMenuClicked()
    {
        //关闭计分UI的画布
        scoringScreenCanvas.enabled = false;
        //加载主菜单场景
        SceneLoader.Instance.LoadMainMenuScene();
    }

    //提前按钮功能函数
    void OnButtonSubmitClicked()
    {
        //文本输入内容非空
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            //修改当前玩家姓名，传入文本输入框内容
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }
}
