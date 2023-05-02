using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region SCORE DISPLAY

    //公有玩家得分属性
    public int Score => _score;
    //玩家得分
    private int _score;
    //??????????÷?
    private int _currentScore;
    //????????????
    [SerializeField] private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);
    //???????ú???????????????????????????????0
    public void ResetScore()
    {
        //???÷??0
        _score = 0;
        //????÷??0
        _currentScore = 0;
        //????÷???????
        ScoreDisplay.UpdateText(_score);
    }
    //????÷????
    public void AddScore(int scorePoint)
    {
        //??????? ???? ????÷??
        _currentScore += scorePoint;
        //???÷???????Э??
        StartCoroutine(nameof(AddScoreCoroutine));
        //????÷????
        ScoreDisplay.UpdateText(_score);
    }
    //????????Э????????????????????仯???????
    IEnumerator AddScoreCoroutine()
    {
        //???????????????????????
        ScoreDisplay.ScaleText(scoreTextScale);
        //???÷? С?? ????÷?? ???
        while (_score<_currentScore)
        {
            //???÷???????1??
            _score += 1;
            //???·?????????
            ScoreDisplay.UpdateText(_score);
            //??????????
            yield return null;
        }
        //????????????????????????1
        ScoreDisplay.ScaleText(Vector3.one);
    }

    #endregion

    #region HIGH SCORE SYSTEM

    //玩家分数 自家义类
    [System.Serializable] public class PlayerScore
    {
        //玩家得分
        public int score;
        //玩家ID
        public string playerName;
        //构造函数便于初始化
        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }
    //玩家得分数据类
    [System.Serializable] public class PlayerScoreData
    {
         //玩家得分类列表
         public List<PlayerScore> list = new List<PlayerScore>();
    }
    
    //存档文件名字符串
    private readonly string SaveFileName = "player_score.json";
    //玩家ID
    private string playerName = "No Name";

    //判断是否取得新高分 当前得分值是否大于玩家得分列表中第十位的得分值
    public bool HasNewHighScore => _score > LoadPlayerScoreData().list[9].score;
    //设置玩家名称变量
    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }
    //存储玩家得分数据
    public void SavePlayerScoreData()
    {
        //读取玩家得分数据并记录
        var playerScoreData = LoadPlayerScoreData();
        //记录本轮玩家得分和ID
        playerScoreData.list.Add(new PlayerScore(_score,playerName));
        //对列表中的得分数据进行排序
        playerScoreData.list.Sort((x,y)=>y.score.CompareTo(x.score));
        //调用存档系统保存函数，存储排序后的玩家得分数据列表
        SaveSystem.SaveByJson(SaveFileName,playerScoreData);
    }
    //加载玩家得分数据
    public PlayerScoreData LoadPlayerScoreData()
    {
        //新玩家得分数据列表
        var playerScoreData = new PlayerScoreData();
        //若存档文件存在
        if(SaveSystem.SaveFileExists(SaveFileName))
            //调用存档系统 读取玩家数据
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(SaveFileName);
        else //若存档文件不存在，即玩家第一次进行游戏
        {
            while (playerScoreData.list.Count<10)
                //新建玩家得分数据，存入列表中
                playerScoreData.list.Add(new PlayerScore(0,playerName));
            //存储玩家得分列表
            SaveSystem.SaveByJson(SaveFileName,playerScoreData);
        }
        //返回玩家得分数据
        return playerScoreData;
    }
    
    #endregion

}
