using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    //背景滚动速度
    [SerializeField]Vector2 scrollVelocity;
    //用于获取材质组件
    Material material;
    void Awake()
    {
        //得到四边形渲染器组件的材质
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        //游戏状态不为结束状态，才每帧卷动背景
        while (GameManager.GameState!=GameState.GameOver)
        {
            //改变材质的主纹理的Offset值
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;
            yield return null;
        }
    }

    // private void Update()
    // {
    //     //改变材质的主纹理的Offset值
    //     material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    // }
    
}
