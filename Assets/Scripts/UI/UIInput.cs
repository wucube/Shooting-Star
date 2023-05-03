using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// UI输入控制
/// </summary>
public class UIInput : Singleton<UIInput>
{
    //玩家输入类变量
    [SerializeField] private PlayerInput playerInput;
    //输入系统的UI输入模块变量
    private InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        //取得输入系统的UI输入模块
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        //先禁用UI输入组件
        UIInputModule.enabled = false;
    }
    //选中UI函数
    //Selectable类是所有可被选中的Unity UI类的基类
    public void SelectUI(Selectable UIObject)
    {
        //选中UI
        UIObject.Select();
        //将UI设置到正确的状态
        UIObject.OnSelect(null);
        //UI输入模块启用，可以接收UI输入
        UIInputModule.enabled = true;
    }
    //禁用所有UI输入函数
    public void DisableAllUIInputs()
    {
        //禁用所有玩家输入
        playerInput.DisableAllInputs();
        //禁用输入模块组件
        UIInputModule.enabled = false;
    }
}
