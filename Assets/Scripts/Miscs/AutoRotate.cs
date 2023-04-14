using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 自动旋转
/// </summary>
public class AutoRotate : MonoBehaviour
{
    /// <summary>
    /// 旋转速度
    /// </summary>
    [SerializeField] private float speed = 360f;

    /// <summary>
    /// 旋转角度
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector3 angle;

    void Update()
    {
        transform.Rotate(angle * speed * Time.deltaTime);
    }
}
