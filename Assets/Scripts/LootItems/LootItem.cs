using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LootItem : MonoBehaviour
{
    //道具移动速度变量
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    
    //默认拾取音效
    [SerializeField] protected AudioData defaultPickUpSFX;
    //播放拾取动画的条件参数，转为哈希值
    private int pickUpStateID = Animator.StringToHash("PickUp");
    //道具上的动画器组件
    private Animator animator;
    //拾取音效变量
    protected AudioData pickUpSFX;
    //玩家对象变量
    protected Player player;
    
    //拾取道具的文本信息
    protected Text lootMessage;

    private void Awake()
    {
        //初始化动画器组件变量
        animator = GetComponent<Animator>();
        //初始化玩家对象变量
        player = FindObjectOfType<Player>();
        //初始化文本信息，传入True，获取禁用的组件
        lootMessage = GetComponentInChildren<Text>(true);
        //初始化领取音效变量，赋予默认音效的值
        pickUpSFX = defaultPickUpSFX;
    }

    private void OnEnable()
    {
        //启用道具移动协程
        StartCoroutine(MoveCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    protected virtual void PickUp()
    {
        //道具停止移动
        StopAllCoroutines();
        //播放拾取动画
        animator.Play(pickUpStateID);
        //播放拾取音效
        AudioManager.Instance.PlayerRandomSFX(pickUpSFX);
    }

    IEnumerator MoveCoroutine()
    {
        //随机取得移动速度
        float speed = Random.Range(minSpeed, maxSpeed);
        //移动方向，默认向左飞行
        Vector3 direction = Vector3.left;
        while (true)
        {
            //如果玩家存活
            if (player.isActiveAndEnabled)
            {
                //修改移动方向向量
                direction = (player.transform.position - transform.position).normalized;
            }
            //道具向玩家方向移动
            transform.Translate(direction*speed*Time.deltaTime);
            yield return null;
        }
    }
}
