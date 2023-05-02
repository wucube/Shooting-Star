using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region SCORE DISPLAY

    //������ҵ÷�����
    public int Score => _score;
    //��ҵ÷�
    private int _score;
    //??????????��?
    private int _currentScore;
    //????????????
    [SerializeField] private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);
    //???????��???????????????????????????????0
    public void ResetScore()
    {
        //???��??0
        _score = 0;
        //????��??0
        _currentScore = 0;
        //????��???????
        ScoreDisplay.UpdateText(_score);
    }
    //????��????
    public void AddScore(int scorePoint)
    {
        //??????? ???? ????��??
        _currentScore += scorePoint;
        //???��???????��??
        StartCoroutine(nameof(AddScoreCoroutine));
        //????��????
        ScoreDisplay.UpdateText(_score);
    }
    //????????��????????????????????��???????
    IEnumerator AddScoreCoroutine()
    {
        //???????????????????????
        ScoreDisplay.ScaleText(scoreTextScale);
        //???��? ��?? ????��?? ???
        while (_score<_currentScore)
        {
            //???��???????1??
            _score += 1;
            //???��?????????
            ScoreDisplay.UpdateText(_score);
            //??????????
            yield return null;
        }
        //????????????????????????1
        ScoreDisplay.ScaleText(Vector3.one);
    }

    #endregion

    #region HIGH SCORE SYSTEM

    //��ҷ��� �Լ�����
    [System.Serializable] public class PlayerScore
    {
        //��ҵ÷�
        public int score;
        //���ID
        public string playerName;
        //���캯�����ڳ�ʼ��
        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }
    //��ҵ÷�������
    [System.Serializable] public class PlayerScoreData
    {
         //��ҵ÷����б�
         public List<PlayerScore> list = new List<PlayerScore>();
    }
    
    //�浵�ļ����ַ���
    private readonly string SaveFileName = "player_score.json";
    //���ID
    private string playerName = "No Name";

    //�ж��Ƿ�ȡ���¸߷� ��ǰ�÷�ֵ�Ƿ������ҵ÷��б��е�ʮλ�ĵ÷�ֵ
    public bool HasNewHighScore => _score > LoadPlayerScoreData().list[9].score;
    //����������Ʊ���
    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }
    //�洢��ҵ÷�����
    public void SavePlayerScoreData()
    {
        //��ȡ��ҵ÷����ݲ���¼
        var playerScoreData = LoadPlayerScoreData();
        //��¼������ҵ÷ֺ�ID
        playerScoreData.list.Add(new PlayerScore(_score,playerName));
        //���б��еĵ÷����ݽ�������
        playerScoreData.list.Sort((x,y)=>y.score.CompareTo(x.score));
        //���ô浵ϵͳ���溯�����洢��������ҵ÷������б�
        SaveSystem.SaveByJson(SaveFileName,playerScoreData);
    }
    //������ҵ÷�����
    public PlayerScoreData LoadPlayerScoreData()
    {
        //����ҵ÷������б�
        var playerScoreData = new PlayerScoreData();
        //���浵�ļ�����
        if(SaveSystem.SaveFileExists(SaveFileName))
            //���ô浵ϵͳ ��ȡ�������
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(SaveFileName);
        else //���浵�ļ������ڣ�����ҵ�һ�ν�����Ϸ
        {
            while (playerScoreData.list.Count<10)
                //�½���ҵ÷����ݣ������б���
                playerScoreData.list.Add(new PlayerScore(0,playerName));
            //�洢��ҵ÷��б�
            SaveSystem.SaveByJson(SaveFileName,playerScoreData);
        }
        //������ҵ÷�����
        return playerScoreData;
    }
    
    #endregion

}
