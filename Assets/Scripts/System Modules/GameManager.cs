using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : PersistentSingleton<GameManager>
{
    /// <summary>
    /// 游戏结束事件
    /// </summary>
    public static UnityAction onGameOver;
    
    /// <summary>
    /// 游戏状态
    /// </summary>
    /// <value></value>
    public static GameState GameState
    {
        get => Instance.gameState;
        set => Instance.gameState = value;
    }

    [SerializeField] private GameState gameState = GameState.Playing;
}

/// <summary>
/// 游戏状态枚举
/// </summary>
public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}