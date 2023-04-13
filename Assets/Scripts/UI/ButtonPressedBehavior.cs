using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPressedBehavior : StateMachineBehaviour
{
    //���о�̬�ֵ䣬��Ϊ��ť���֣�ֵΪ����������ί�� - ��ť���ܱ�
    public static Dictionary<string, Action> buttonFunctionTable;

    private void Awake()
    {
        //��ʼ���ֵ�
        buttonFunctionTable = new Dictionary<string, Action>();
    }

   //״̬���뺯��������״̬�ĵ�һ֡����
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //������������
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
        //���� ���Ű��¶����İ�ť ��Ӧ��ί��
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
