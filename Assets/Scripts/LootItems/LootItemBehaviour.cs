using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemBehaviour : StateMachineBehaviour
{
    //״̬�˳�����
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //���ö����������صĶ���
        animator.gameObject.SetActive(false);
    }
}
