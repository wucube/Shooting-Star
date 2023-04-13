using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频管理器
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    /// <summary>
    /// 音效源
    /// </summary>
    [SerializeField] private AudioSource sfxPlayer;

    /// <summary>
    /// 最小音量
    /// </summary>
    private const float MinPitch = 0.9f;
    /// <summary>
    /// 最大音量
    /// </summary>
    private const float MaxPitch = 1.1f;

    /// <summary>
    /// 播放声效
    /// </summary>
    /// <param name="audioData"></param>
    public void PlaySFX(AudioData audioData)
    {
        sfxPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }
    /// <summary>
    /// 随机音量播放声效
    /// </summary>
    /// <param name="audioData"></param>
    public void PlayerRandomSFX(AudioData audioData)
    {
        sfxPlayer.pitch = Random.Range(MinPitch, MaxPitch);
        PlaySFX(audioData);
    }
    /// <summary>
    /// 随机播放不同音效
    /// </summary>
    public void PlayerRandomSFX(AudioData[] audioData)
    {
        
        PlayerRandomSFX(audioData[Random.Range(0,audioData.Length)]);
    }
}
