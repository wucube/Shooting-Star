using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战利品行为脚本
/// </summary>
public class LootItemBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
