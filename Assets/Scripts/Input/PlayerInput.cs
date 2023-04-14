using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// 玩家输入类
/// </summary>
[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions, InputActions.IPauseMenuActions, InputActions.IGameOverScreenActions
{
    /// <summary>
    /// 移动输入事件
    /// </summary>
    public event UnityAction<Vector2> onMove = delegate {  };

    /// <summary>
    /// 停止移动输入事件
    /// </summary>
    public event UnityAction onStopMove = delegate { }; 

    /// <summary>
    /// 开火输入事件
    /// </summary>
    public event UnityAction onFire  =delegate { };

    /// <summary>
    /// 停止开火输入输入事件
    /// </summary>
    public event UnityAction onStopFire = delegate { };

    /// <summary>
    /// 闪避输入事件
    /// </summary>
    public event UnityAction onDodge = delegate { };

    /// <summary>
    /// 开启能量爆发输入事件
    /// </summary>
    public event UnityAction onOverdrive = delegate { };

    /// <summary>
    /// 暂停输入事件
    /// </summary>
    public event UnityAction onPause= delegate { };

    /// <summary>
    /// 取消暂停输入事件
    /// </summary>
    public event UnityAction onUnpause= delegate { };

    /// <summary>
    /// 发射导弹输入事件
    /// </summary>
    public event UnityAction onLaunchMissile = delegate { };
    
    /// <summary>
    /// 确定游戏结束输入事件
    /// </summary>
    public event UnityAction onConfirmGameOver = delegate { };
    
    /// <summary>
    /// 输入动作类
    /// </summary>
    InputActions inputActions;
    void OnEnable()
    {
        inputActions = new InputActions();

        //玩家输入类实例设为三种输入动作表的回调
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        //取消所有输入
        DisableAllInputs();
    }
    
    /// <summary>
    /// 切换输入动作表
    /// </summary>
    /// <param name="actionMap">输入动作表</param>
    /// <param name="isUIInput">是否为UI输入</param>
    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        //禁用输入动作类
        inputActions.Disable();
        
        actionMap.Enable();
        
        if (isUIInput)
        {
            //光标可见
            Cursor.visible = true;
            //光标不锁定
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// 切换到动态更新模式
    /// </summary>
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    /// <summary>
    /// 切换到固定更新模式
    /// </summary>
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    
    /// <summary>
    /// 取消所有输入
    /// </summary>
    public void DisableAllInputs() => inputActions.Disable();
    
    /// <summary>
    /// 启用GamePlay输入
    /// </summary>
    public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay, false);
    
    /// <summary>
    /// 启用游戏暂停菜单的输入
    /// </summary>
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);
    
    /// <summary>
    /// 启用游戏结束界面的输入
    /// </summary>
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);

    /// <summary>
    /// 移动输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) 
            onMove.Invoke(context.ReadValue<Vector2>());
        
        if (context.canceled)
            onStopMove.Invoke();
        
    }
    
    /// <summary>
    /// 开火输入事件处理器
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
    /// 闪避输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed) 
            onDodge.Invoke();
    }
   
    /// <summary>
    /// 能量爆发启用输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed) 
            onOverdrive.Invoke();
    }

    /// <summary>
    /// 暂停输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            onPause.Invoke();
    }
    
    /// <summary>
    /// 取消暂停输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
            onUnpause.Invoke();
    }
    
    /// <summary>
    /// 发射导弹输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed) 
            onLaunchMissile.Invoke();
    }
    
    /// <summary>
    /// 确认游戏结束输入事件处理器
    /// </summary>
    /// <param name="context"></param>
    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {

        if(context.performed) 
            onConfirmGameOver.Invoke();
    }
}
