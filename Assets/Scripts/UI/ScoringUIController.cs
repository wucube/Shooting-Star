using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoringUIController : MonoBehaviour
{
    [Header("===== BACKGROUND ======")]
    //����ͼƬImage���
    [SerializeField] private Image background;
    //����Ҫ��ʾ��ͼƬ����
    [SerializeField] private Sprite[] backgroundImages;

    [Header("===== SCORING SCREEN ======")] 
    //���ֻ��滭�����
    [SerializeField] private Canvas scoringScreenCanvas;
    //��ҷ����ı����
    [SerializeField] private Text playerScoreText;
    //���˵���ť���
    [SerializeField] private Button buttonMainMenu;
    //�߷����а�����
    [SerializeField] private Transform highScoreLeaderboardContainer;

    [Header("===== HIGH SCORE SCREEN ======")]
    //�¸߷�UI�������
    [SerializeField] private Canvas newHighScoreScreenCanvas;
    //��ť����
    [SerializeField] private Button buttonCancel;
    [SerializeField] private Button buttonSubmit;
    //�ı���������
    [SerializeField] private InputField playerNameInputField;

    private void Start()
    {
        //һ����Ʒֳ������ͻ���ʾ���
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //���������ʾ����ͼƬ����
        ShowRandomBackground();
        //����Ƿ�ȡ�����¸߷�
        if (ScoreManager.Instance.HasNewHighScore)
        {
            //��ʾ�¸߷ֻ���
            ShowNewHighScoreScreen();
        }
        else //û��ȡ���¸߷ֻ��棬��ʾ�÷�UI
            ShowScoringScreen();
        //�Ǽǰ�ť���ܺ���
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name,OnButtonMainMenuClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.name,OnButtonSubmitClicked);
        //ȡ����ť���ܾ��ǹر��¸߷ֻ���
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.name,HideNewHighScoreScreen);
        //��Ϸ����Ʒ�״̬
        GameManager.GameState = GameState.Scoring;
    }
    //��ʾ�¸߷ֻ���
    void ShowNewHighScoreScreen()
    {
        //����UI����
        newHighScoreScreenCanvas.enabled = true;
        //ѡ��ȡ����ť
        UIInput.Instance.SelectUI(buttonCancel);
    }

    private void OnDisable()
    {
        //��հ�ť���ܱ��ֵ�
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    //��ʾ�������ͼƬ
    void ShowRandomBackground()
    {
        //���һ�ű���ͼƬ�����е�ͼƬ����ֵ������ͼƬ�����sprite
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
        
    }
    
    //�ر��·ֻ���UI
    void HideNewHighScoreScreen()
    {
        //����UI����
        newHighScoreScreenCanvas.enabled = false;
        //������ҵ÷�����
        ScoreManager.Instance.SavePlayerScoreData();
        //��ʾ�������
        ShowRandomBackground();
        //��ʾ�������
        ShowScoringScreen();
        
    }
    //��ʾ���ֻ���
    void ShowScoringScreen()
    {
        //�����Ʒֻ���UI
        scoringScreenCanvas.enabled = true;
        //��ʾ������յ÷�
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        //ѡ�����˵���ť
        UIInput.Instance.SelectUI(buttonMainMenu);
       
        //���¸߷����а�
        
        UpdateHighScoreLeaderboard();
    }

    //���¸߷����а�����
    void UpdateHighScoreLeaderboard()
    {
        //��ȡ����¼��ҵ÷������б�
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;
        //���б�����һһ��ʾ��UI��
        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            //λ��ȡ��ÿ��UI�Ӷ���
            var child = highScoreLeaderboardContainer.GetChild(i);
            //UI�Ӷ����ҵ��Լ����ı��Ӷ���������ݸ�ֵ���ı����
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }
    //���˵���ť���ܺ���
    void OnButtonMainMenuClicked()
    {
        //�رռƷ�UI�Ļ���
        scoringScreenCanvas.enabled = false;
        //�������˵�����
        SceneLoader.Instance.LoadMainMenuScene();
    }

    //��ǰ��ť���ܺ���
    void OnButtonSubmitClicked()
    {
        //�ı��������ݷǿ�
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            //�޸ĵ�ǰ��������������ı����������
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }
        HideNewHighScoreScreen();
    }
}
