using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemBehaviour : StateMachineBehaviour
{
    //状态退出函数
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //禁用动画器所挂载的对象
        animator.gameObject.SetActive(false);
    }
}
