using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
/// <summary>
/// UI界面的输入
/// </summary>
public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput playerInput;
    
    /// <summary>
    /// UI输入模块
    /// </summary>
    private InputSystemUIInputModule UIInputModule;

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }
    
    /// <summary>
    /// 选择UI
    /// </summary>
    /// <param name="UIObject">继承Selectable类的组件对象</param>
    public void SelectUI(Selectable UIObject)
    {
        //选中传入的UI对象
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }
    /// <summary>
    /// 取消所有UI输入
    /// </summary>
    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled = false;
    }
}
