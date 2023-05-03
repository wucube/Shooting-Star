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
    public int Score => _score;
    private int _score;
    
    /// <summary>
    /// 当前分数
    /// </summary>
    private int _currentScore;
    
    [SerializeField] private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);
   
    public void ResetScore()
    {
        _score = 0;
        _currentScore = 0;

        ScoreDisplay.UpdateText(_score);
    }
    
    public void AddScore(int scorePoint)
    {
        _currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
        ScoreDisplay.UpdateText(_score);
    }
   
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (_score<_currentScore)
        {
            _score += 1;
           
            ScoreDisplay.UpdateText(_score);
         
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
    [System.Serializable] 
    public class PlayerScoreData
    {
         public List<PlayerScore> list = new List<PlayerScore>();
    }
    
    private readonly string SaveFileName = "player_score.json";
    
    private string playerName = "No Name";

    public bool HasNewHighScore => _score > LoadPlayerScoreData().list[9].score;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }
    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
       
        playerScoreData.list.Add(new PlayerScore(_score,playerName));
        
        playerScoreData.list.Sort((x,y)=>y.score.CompareTo(x.score));
        
        SaveSystem.SaveByJson(SaveFileName,playerScoreData);
    }
    
    public PlayerScoreData LoadPlayerScoreData()
    {
       
        var playerScoreData = new PlayerScoreData();
  
        if(SaveSystem.SaveFileExists(SaveFileName))
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(SaveFileName);
        else 
        {
            while (playerScoreData.list.Count<10)
                playerScoreData.list.Add(new PlayerScore(0,playerName));
            
            SaveSystem.SaveByJson(SaveFileName,playerScoreData);
        }
        
        return playerScoreData;
    }
    
    #endregion

}
