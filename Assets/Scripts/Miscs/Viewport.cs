using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Viewport : Singleton<Viewport>
{
    //���λ�� x��y��ȡֵ��Χ
    float minX;
    float maxX;
    float minY;
    float maxY;
    
    //�ӿ����ĵ��X��ֵ
    float middleX;
    
    //��������MaxX
    public float MaxX => maxX;
    void Start()
    {
        Camera mainCamera = Camera.main;
        
        //�ӿ�����ת��������
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));//�ӿ����½�����ת����
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));//�ӿ����Ͻ�����ת����
        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;//�ӿ����ĵ��X������תΪ����ϵ����
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }
    
    //����ƶ����ӿڷ�Χ��
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
    {
        //��ʱ�����洢λ��
        Vector3 position = Vector3.zero;
        //�������X��ֵ���ƶ���Χ
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        //�������Y��ֵ���ƶ���Χ
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);
        return position;
    }
    
    //�����������λ�ã��ھ�ͷ����Ұ벿��
    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
    
    //�л��ھ�ͷ���Ұ벿�����λ��
    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY+paddingX, maxY - paddingY);
        return position;
    }
    //�л�����ھ�ͷ��λ��
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingX, maxY - paddingY);
        return position;
    }
}
