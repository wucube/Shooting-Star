using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    //音源组件变量，音效播放器
    [SerializeField] private AudioSource sFXPlayer;
    //最小音高常量
    private const float MinPitch = 0.9f;
    //最大音高常量
    private const float MaxPitch = 1.1f;

    //音效播放函数 适用于播放UI音效
    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }
    //随机播放不同音效，相同音效不同音高 适用于会连续重复播放的音效，如子弹发射音效
    public void PlayerRandomSFX(AudioData audioData)
    {
        //随机播放不同音高的音效
        sFXPlayer.pitch = Random.Range(MinPitch, MaxPitch);
        PlaySFX(audioData);
    }
    //随机播放不同音效函数重载，传入音频数据集合
    public void PlayerRandomSFX(AudioData[] audioData)
    {
        //从音频数据集合中随机一个音频资料并播放
        PlayerRandomSFX(audioData[Random.Range(0,audioData.Length)]);
    }
}
