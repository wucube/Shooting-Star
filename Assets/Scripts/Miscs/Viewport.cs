using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// 视口类
/// </summary>
public class Viewport : Singleton<Viewport>
{
    //视口范围内的坐标极值
    float minX;
    float maxX;
    float minY;
    float maxY;

    /// <summary>
    /// 视口中心点的X轴值
    /// </summary>
    float middleX;
    
    /// <summary>
    /// 视口范围内的最大x轴值
    /// </summary>
    public float MaxX => maxX;
    void Start()
    {
        Camera mainCamera = Camera.main;
        
        //视口左下角的坐标：视口坐标转世界坐标
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        //视口右上角的坐标：视口坐标转世界坐标
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }
    
    /// <summary>
    /// 玩家机体可移动的位置
    /// </summary>
    /// <param name="playerPosition">玩家位置</param>
    /// <param name="paddingX">机体中心的水平边距</param>
    /// <param name="paddingY">机体中心的垂直边距</param>
    /// <returns></returns>
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }
    
    /// <summary>
    /// 敌人随机出生的位置
    /// </summary>
    /// <param name="paddingX">机体的水平边距</param>
    /// <param name="paddingY">机体的垂直边距</param>
    /// <returns></returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;

        //机体从屏幕外进入屏幕内(X轴)
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
    
    /// <summary>
    /// 视口右半边的随机位置
    /// </summary>
    /// <param name="paddingX">机体的水平边距</param>
    /// <param name="paddingY">机体的垂直边距</param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        
        //X轴的值在视口右半边范围内
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY+paddingX, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// 敌人随机移动的位置
    /// </summary>
    /// <param name="paddingX"></param>
    /// <param name="paddingY"></param>
    /// <returns></returns>
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingX, maxY - paddingY);
        
        return position;
    }
}
