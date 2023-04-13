using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    //������������
    [SerializeField] private PlayerInput input;
    //HUD��������
    [SerializeField] private Canvas HUDCanvas;
    //ȷ����Ϸ������Ч
    [SerializeField] private AudioData confirmGameOverSound;
    //��Ϸ���������˳�״̬ID
    private int exitStateID = Animator.StringToHash("GameOverScreenExit");
    
    //��Ϸ��������UI�������
    private Canvas _canvas;
    //��������������ƶ����Ĳ���
    private Animator _animator;

    private void Awake()
    {
        //��ȡ����
        _canvas = GetComponent<Canvas>();
        //��ȡ������
        _animator = GetComponent<Animator>();
        //���û����붯����
        _canvas.enabled = false;
        _animator.enabled = false;
    }

    private void OnEnable()
    {
        //������Ϸ����ί��
        GameManager.onGameOver += OnGameOver;
        //������Ұ����¼�
        input.onConfirmGameOver += OnConfirmGameOver;
    }

    private void OnDisable()
    {
        //�˶���Ϸ����ί��
        GameManager.onGameOver -= OnGameOver;
        //�˶���Ұ����¼�
        input.onConfirmGameOver -= OnConfirmGameOver;
    }

    //ȷ����Ϸ���������¼������� 
    private void OnConfirmGameOver()
    {
        //����ȷ�Ͻ�����Ч����
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        //���������������
        input.DisableAllInputs();
        //��Ϸ����������˳�����
        _animator.Play(exitStateID);
        //���ؼƷֳ���
        SceneLoader.Instance.LoadScoringScene(); // TODO
    }
    //��Ϸ����ί�д�����
    void OnGameOver()
    {
        //�ر�HUD����
        HUDCanvas.enabled = false;
        //������Ϸ��������
        _canvas.enabled = true;
        //���ö�����������Ŷ���
        _animator.enabled = true;
        //���������������
        input.DisableAllInputs();
    }
    
    //������Ϸ�����������룬�����¼�
    void EnableGameOverScreenInput()
    {
        //�л�����Ϸ����������
        input.EnableGameOverScreenInput();
    }
}
