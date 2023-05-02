using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    //��Ϊ��̬ί��ʹ�÷���ʹ�ã�Ҳ���Բ�����ͨ�����������
    public static UnityAction onGameOver;
    
    //��Ϸ״̬����
    public static GameState GameState
    {
        get => Instance.gameState;
        set => Instance.gameState = value;
    }
    //��Ϸ״̬��Ĭ��Ϊ��Ϸ����״̬
    [SerializeField] private GameState gameState = GameState.Playing;
}
//��Ϸ״����ö��
public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}