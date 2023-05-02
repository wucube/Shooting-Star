using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    //??????
    [SerializeField] private float speed = 360f;
    //??????
    [SerializeField] private Vector3 angle;

    void Update()
    {
        transform.Rotate(angle*speed*Time.deltaTime);
    }
}
