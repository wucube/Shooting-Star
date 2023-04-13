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
    
    //移动事件成员 ,赋值空委托作为初始值，调用时不再做空值检查
    public event UnityAction<Vector2> onMove = delegate {  };
    //停止移动事件成员
    public event UnityAction onStopMove = delegate { }; 
    //玩家开火事件
    public event UnityAction onFire  =delegate { };
    //玩家停止开火事件
    public event UnityAction onStopFire = delegate { };
    //玩家闪避事件
    public event UnityAction onDodge = delegate { };
    //能量爆发事件
    public event UnityAction onOverdrive = delegate { };
    //暂停事件
    public event UnityAction onPause= delegate { };
    //结束暂停事件
    public event UnityAction onUnpause= delegate { };
    //发射导弹事件
    public event UnityAction onLaunchMissile = delegate { };
    
    //确认游戏结束事件
    public event UnityAction onConfirmGameOver = delegate { };
    
    //InputActions类的引用
    InputActions _inputActions;
    void OnEnable()
    {
        //初始化InputActions类的引用
        _inputActions = new InputActions();
        //登记GamePlay动作表的回调函数，传入IGamePlay接口
        _inputActions.GamePlay.SetCallbacks(this);
        //登记暂停菜单动作的回调函数，传入 暂停菜单 接口
        _inputActions.PauseMenu.SetCallbacks(this);
        //登记游戏结束动作的回调函数
        _inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        //禁用玩家输入
        DisableAllInputs();
    }
    
    //切换动作表函数
    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        //禁用 输入类中的所有动作表
        _inputActions.Disable();
        //启用 目标动作表 
        actionMap.Enable();
        //如果启用的动作表用于UI输入
        if (isUIInput)
        {
            //光标可见
            Cursor.visible = true;
            //不锁定光标
            Cursor.lockState = CursorLockMode.None;
        }
        //如果启用的动作表不是UI输入
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //切换到动态更新模式
    public void SwitchToDynamicUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    //切换到固定更新模式
    public void SwitchToFixedUpdateMode() =>
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    
    //禁用所用输入动作函数，供外部使用
    public void DisableAllInputs() => _inputActions.Disable();
    
    //启用GamePlay动作表的函数,直接调用动作表切换函数启用GamePlay动作表
    public void EnableGameplayInput() => SwitchActionMap(_inputActions.GamePlay, false);
    
    //启用暂停菜单动作表
    public void EnablePauseMenuInput() => SwitchActionMap(_inputActions.PauseMenu, true);
    //启用游戏结束动作表
    public void EnableGameOverScreenInput() => SwitchActionMap(_inputActions.GameOverScreen, false);
    //移动输入动作接口函数
    public void OnMove(InputAction.CallbackContext context)
    {
        //GamePlay动作表接收到移动输入信号
        //玩家家按住按键时，角色持续移动
        if (context.performed) 
        {
            //调用onMove事件
            //传入输入动作读取到的二维向量值
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        //松开按键时，角色停止移动
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }
    
    //开火输入动作接口函数
    public void OnFire(InputAction.CallbackContext context)
    {
        //按住按键，调用开火事件
        if (context.performed)
            onFire.Invoke();
        //机型按键，调用停火事件
        if (context.canceled)
            onStopFire.Invoke();
    }
    //闪避动作接口函数
    public void OnDodge(InputAction.CallbackContext context)
    {
        //按下闪避键则调用闪避事件
        if(context.performed) onDodge.Invoke();
    }
    //能量爆发动作接口函数
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        //按下能量爆发键，调用能量爆发事件
        if (context.performed) onOverdrive.Invoke();
    }

    //暂停动作接口函数
    public void OnPause(InputAction.CallbackContext context)
    {
        //按下暂停键，触发暂停事件
        if (context.performed)
            onPause.Invoke();
    }
    //结束暂停动作接口函数
    public void OnUnpause(InputAction.CallbackContext context)
    {
        //按下暂停键，触发结束暂停事件
        if (context.performed)
            onUnpause.Invoke();
    }
    //发射导弹动作接口函数
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        //按下导弹发射键，调用发射导弹事件
        if (context.performed) onLaunchMissile.Invoke();
    }
    
    //确认游戏结束动作接口函数
    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        //按下确认键，调用确认游戏结束事件
        if(context.performed) onConfirmGameOver.Invoke();
    }
}
