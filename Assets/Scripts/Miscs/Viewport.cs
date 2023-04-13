using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Viewport : Singleton<Viewport>
{
    //玩家位置 x、y轴取值范围
    float minX;
    float maxX;
    float minY;
    float maxY;
    
    //视口中心点的X轴值
    float middleX;
    
    //公有属性MaxX
    public float MaxX => maxX;
    void Start()
    {
        Camera mainCamera = Camera.main;
        
        //视口坐标转世界坐标
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//视口左下角坐标转世界
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));//视口右上角坐标转世界
        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;//视口中心点的X点坐标转为世界系坐标
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }
    
    //玩家移动在视口范围内
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        //临时变量存储位置
        Vector3 position = Vector3.zero;
        //限制玩家X轴值的移动范围
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        //限制玩家Y轴值的移动范围
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);
        return position;
    }
    
    //敌人随机出生位置，在镜头外的右半部分
    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
    
    //敌机在镜头内右半部分随机位置
    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY+paddingX, maxY - paddingY);
        return position;
    }
    //敌机随机在镜头内位置
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingX, maxY - paddingY);
        return position;
    }
}
