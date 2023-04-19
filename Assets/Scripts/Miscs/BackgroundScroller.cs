using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景视图滚动器
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    
    /// <summary>
    /// 滚动速度
    /// </summary>
    [SerializeField]Vector2 scrollVelocity;

    Material material;
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    
    IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            //背景视图材质的主贴图位置不断偏移
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;

            yield return null;
        }
    }

    // 在Update中不断滚动背景视图
    // private void Update()
    // {
    //     
    //     material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    // }
    
}
