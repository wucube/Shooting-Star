using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    //设为静态委托使用访问使用，也可以不设置通过单例类访问
    public static UnityAction onGameOver;
    
    //游戏状态属性
    public static GameState GameState
    {
        get => Instance.gameState;
        set => Instance.gameState = value;
    }
    //游戏状态，默认为游戏运行状态
    [SerializeField] private GameState gameState = GameState.Playing;
}
//游戏状公有枚举
public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}