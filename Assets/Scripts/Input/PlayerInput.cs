using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// �������ϵͳ
/// </summary>
[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : 
    ScriptableObject, 
    InputActions.IGamePlayActions,
    InputActions.IPauseMenuActions,
    InputActions.IGameOverScreenActions
{
    //public event UnityAction<Vector2> onMove = delegate { };
    
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire  =delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause= delegate { };
    public event UnityAction onUnpause= delegate { };
    public event UnityAction onLaunchMissile = delegate { };
    public event UnityAction onConfirmGameOver = delegate { };
    
    InputActions inputActions;
    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisableAllInputs();
    }

    /// <summary>
    /// �л����붯����
    /// </summary>
    /// <param name="actionMap"></param>
    /// <param name="isUIInput"></param>
    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchToDynamicUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    public void SwitchToFixedUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    
    public void DisableAllInputs() => inputActions.Disable();
    
    public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay, false);
    
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);

    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);

    /// <summary>
    /// �ƶ�����������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }
    /// <summary>
    /// ������������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            onFire.Invoke();
        if (context.canceled)
            onStopFire.Invoke();
    }
    /// <summary>
    /// ���ܶ���������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed) onDodge.Invoke();
    }
   
   /// <summary>
   /// ������������������뺯��
   /// </summary>
   /// <param name="context"></param>
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed) onOverdrive.Invoke();
    }

    /// <summary>
    /// ��ͣ����������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            onPause.Invoke();
    }
    
    /// <summary>
    /// ������ͣ����������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
            onUnpause.Invoke();
    }
   
    /// <summary>
    /// ���䵼������������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed) onLaunchMissile.Invoke();
    }
    
    /// <summary>
    /// ȷ�Ͻ�������������뺯��
    /// </summary>
    /// <param name="context"></param>
    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if(context.performed) onConfirmGameOver.Invoke();
    }
}
