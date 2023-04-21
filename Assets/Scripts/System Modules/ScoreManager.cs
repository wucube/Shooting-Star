using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 分数管理器
/// </summary>
public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region SCORE DISPLAY
    public int Score => score;
    private int score;

    /// <summary>
    /// 当前分数
    /// </summary>
    private int currentScore;
    
    /// <summary>
    /// 分数文本的缩放
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);
    
    /// <summary>
    /// 分数重置
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }
    
    /// <summary>
    /// 添加分数
    /// </summary>
    /// <param name="scorePoint"></param>
    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
        ScoreDisplay.UpdateText(score);
    }
    /// <summary>
    /// 添加分数的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (score < currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }
    #endregion

    #region HIGH SCORE SYSTEM

    /// <summary>
    /// 玩家分数类
    /// </summary>
    [System.Serializable] public class PlayerScore
    {
        public int score;
        public string playerName;
        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }
    /// <summary>
    /// 玩家分数数据
    /// </summary>
    [System.Serializable] public class PlayerScoreData
    {
         public List<PlayerScore> list = new List<PlayerScore>();
    }
    
    /// <summary>
    /// 存储文件的名称
    /// </summary>
    private readonly string SaveFileName = "player_score.json";
    /// <summary>
    /// 玩家名
    /// </summary>
    private string playerName = "No Name";
    /// <summary>
    /// 是否有新的高分
    /// </summary>
    /// <returns></returns>
    public bool HasNewHighScore => score > LoadPlayerScoreData().list[9].score;
    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }
    /// <summary>
    /// 存储玩家分数数据
    /// </summary>
    public void SavePlayerScoreData()
    {
        //先加载玩家分类数据
        var playerScoreData = LoadPlayerScoreData();
        //添加新的玩家分数到数据列表中
        playerScoreData.list.Add(new PlayerScore(score,playerName));
        //为分数列表排序
        playerScoreData.list.Sort((x,y) => y.score.CompareTo(x.score));
        //将排好序的分类列表以Json数据方式存储到本地
        SaveSystem.SaveByJson(SaveFileName,playerScoreData);
    }
    
    /// <summary>
    /// 加载玩家分数数据
    /// </summary>
    /// <returns></returns>
    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();
        //如果本地已存在数据，直接加载该数据
        if(SaveSystem.SaveFileExists(SaveFileName))
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(SaveFileName);
        else//否则新建数据
        {
            while (playerScoreData.list.Count < 10)
                playerScoreData.list.Add(new PlayerScore(0, playerName));
            //存储默认的玩家分数数据
            SaveSystem.SaveByJson(SaveFileName, playerScoreData);
        }
        //返回默认的分数信息
        return playerScoreData;
    }
    
    #endregion

}
