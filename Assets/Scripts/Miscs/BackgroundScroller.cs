using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景图滚动
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]Vector2 scrollVelocity;
    Material material;
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        while (GameManager.GameState!=GameState.GameOver)
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }

    // private void Update()
    // {
    //     material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    // }
    
}
