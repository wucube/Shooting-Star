using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] //使类中公有变量曝露到编辑器中
public class AudioData
{
    //音频剪辑文件
    public AudioClip audioClip;
    //音量大小
    public float volume;
}
