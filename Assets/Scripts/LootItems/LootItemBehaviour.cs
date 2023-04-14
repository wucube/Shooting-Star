using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战利器的状态机行为脚本
/// </summary>
public class LootItemBehaviour : StateMachineBehaviour
{
    //状态退出时隐藏战利器
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
