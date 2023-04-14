using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 战利品基类
/// </summary>
public class LootItem : MonoBehaviour
{
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;

    /// <summary>
    /// 战利品默认拾取音效
    /// </summary>
    /// <returns></returns>
    [SerializeField] protected AudioData defaultPickUpSFX;
    
    /// <summary>
    /// 战利品动画名字符串的哈希值
    /// </summary>
    /// <returns></returns>
    private int pickUpStateID = Animator.StringToHash("PickUp");
    private Animator animator;
    protected AudioData pickUpSFX;
    protected Player player;

    /// <summary>
    /// 战利品拾取时的提示信息
    /// </summary>
    protected Text lootMessage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        player = FindObjectOfType<Player>();
        
        lootMessage = GetComponentInChildren<Text>(true);
        
        pickUpSFX = defaultPickUpSFX;
    }
    private void OnEnable()
    {
        StartCoroutine(MoveCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PickUp();
    }

    /// <summary>
    /// 战利品拾取
    /// </summary>
    protected virtual void PickUp()
    {
        StopAllCoroutines();

        animator.Play(pickUpStateID);

        AudioManager.Instance.PlayerRandomSFX(pickUpSFX);
    }
    
    /// <summary>
    /// 战利品移动协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(minSpeed, maxSpeed);

        Vector3 direction = Vector3.left;

        while (true)
        {
            if (player.isActiveAndEnabled)
                direction = (player.transform.position - transform.position).normalized;

            transform.Translate(direction * speed * Time.deltaTime);

            yield return null;
        }
    }
}
