using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 游戏结束时UI
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private AudioData confirmGameOverSound;
    private int exitStateID = Animator.StringToHash("GameOverScreenExit");
    
    private Canvas canvas;
    private Animator animator;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
        canvas.enabled = false;
        animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        input.onConfirmGameOver += OnConfirmGameOver;
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        input.onConfirmGameOver -= OnConfirmGameOver;
    }

    /// <summary>
    /// 确认游戏结束输入的事件处理器
    /// </summary>
    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        input.DisableAllInputs();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene(); 
    }
    /// <summary>
    /// 游戏结束事件处理器
    /// </summary>
    void OnGameOver()
    {
        HUDCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        input.DisableAllInputs();
    }
    void EnableGameOverScreenInput()
    {
        input.EnableGameOverScreenInput();
    }
}
