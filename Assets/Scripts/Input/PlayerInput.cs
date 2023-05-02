using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : 
    ScriptableObject, 
    InputActions.IGamePlayActions,
    InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions
{
    //public event UnityAction<Vector2> onMove = delegate { };
    
    //�ƶ��¼���Ա ,��ֵ��ί����Ϊ��ʼֵ������ʱ��������ֵ���
    public event UnityAction<Vector2> onMove = delegate {  };
    //ֹͣ�ƶ��¼���Ա
    public event UnityAction onStopMove = delegate { }; 
    //��ҿ����¼�
    public event UnityAction onFire  =delegate { };
    //���ֹͣ�����¼�
    public event UnityAction onStopFire = delegate { };
    //��������¼�
    public event UnityAction onDodge = delegate { };
    //���������¼�
    public event UnityAction onOverdrive = delegate { };
    //��ͣ�¼�
    public event UnityAction onPause= delegate { };
    //������ͣ�¼�
    public event UnityAction onUnpause= delegate { };
    //���䵼���¼�
    public event UnityAction onLaunchMissile = delegate { };
    
    //ȷ����Ϸ�����¼�
    public event UnityAction onConfirmGameOver = delegate { };
    
    //InputActions�������
    InputActions _inputActions;
    void OnEnable()
    {
        //��ʼ��InputActions�������
        _inputActions = new InputActions();
        //�Ǽ�GamePlay������Ļص�����������IGamePlay�ӿ�
        _inputActions.GamePlay.SetCallbacks(this);
        //�Ǽ���ͣ�˵������Ļص����������� ��ͣ�˵� �ӿ�
        _inputActions.PauseMenu.SetCallbacks(this);
        //�Ǽ���Ϸ���������Ļص�����
        _inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        //�����������
        DisableAllInputs();
    }
    
    //�л���������
    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        //���� �������е����ж�����
        _inputActions.Disable();
        //���� Ŀ�궯���� 
        actionMap.Enable();
        //������õĶ���������UI����
        if (isUIInput)
        {
            //���ɼ�
            Cursor.visible = true;
            //���������
            Cursor.lockState = CursorLockMode.None;
        }
        //������õĶ�������UI����
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //�л�����̬����ģʽ
    public void SwitchToDynamicUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    //�л����̶�����ģʽ
    public void SwitchToFixedUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    
    //�����������붯�����������ⲿʹ��
    public void DisableAllInputs() => _inputActions.Disable();
    
    //����GamePlay������ĺ���,ֱ�ӵ��ö������л���������GamePlay������
    public void EnableGameplayInput() => SwitchActionMap(_inputActions.GamePlay, false);
    
    //������ͣ�˵�������
    public void EnablePauseMenuInput() => SwitchActionMap(_inputActions.PauseMenu, true);
    //������Ϸ����������
    public void EnableGameOverScreenInput() => SwitchActionMap(_inputActions.GameOverScreen, false);
    //�ƶ����붯���ӿں���
    public void OnMove(InputAction.CallbackContext context)
    {
        //GamePlay��������յ��ƶ������ź�
        //��ҼҰ�ס����ʱ����ɫ�����ƶ�
        if (context.performed) 
        {
            //����onMove�¼�
            //�������붯����ȡ���Ķ�ά����ֵ
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        //�ɿ�����ʱ����ɫֹͣ�ƶ�
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }
    
    //�������붯���ӿں���
    public void OnFire(InputAction.CallbackContext context)
    {
        //��ס���������ÿ����¼�
        if (context.performed)
            onFire.Invoke();
        //���Ͱ���������ͣ���¼�
        if (context.canceled)
            onStopFire.Invoke();
    }
    //���ܶ����ӿں���
    public void OnDodge(InputAction.CallbackContext context)
    {
        //�������ܼ�����������¼�
        if(context.performed) onDodge.Invoke();
    }
    //�������������ӿں���
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        //�����������������������������¼�
        if (context.performed) onOverdrive.Invoke();
    }

    //��ͣ�����ӿں���
    public void OnPause(InputAction.CallbackContext context)
    {
        //������ͣ����������ͣ�¼�
        if (context.performed)
            onPause.Invoke();
    }
    //������ͣ�����ӿں���
    public void OnUnpause(InputAction.CallbackContext context)
    {
        //������ͣ��������������ͣ�¼�
        if (context.performed)
            onUnpause.Invoke();
    }
    //���䵼�������ӿں���
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        //���µ�������������÷��䵼���¼�
        if (context.performed) onLaunchMissile.Invoke();
    }
    
    //ȷ����Ϸ���������ӿں���
    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        //����ȷ�ϼ�������ȷ����Ϸ�����¼�
        if(context.performed) onConfirmGameOver.Invoke();
    }
}
