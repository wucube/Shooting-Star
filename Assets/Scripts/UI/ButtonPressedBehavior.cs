using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 按钮按下的状态机脚本
/// </summary>
public class ButtonPressedBehavior : StateMachineBehaviour
{
    //公有静态字典，键为按钮名字，值为不带参数的委托 - 按钮功能表
    public static Dictionary<string, Action> buttonFunctionTable;

    private void Awake()
    {
        //初始化字典
        buttonFunctionTable = new Dictionary<string, Action>();
    }

   //状态进入函数，进入状态的第一帧调用
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //禁用所有输入
        UIInput.Instance.DisableAllUIInputs();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //调用 播放按下动画的按钮 对应的委托
        buttonFunctionTable[animator.gameObject.name].Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
