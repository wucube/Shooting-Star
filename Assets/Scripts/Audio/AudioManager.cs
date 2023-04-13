using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频管理器
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    //��Դ�����������Ч������
    [SerializeField] private AudioSource sfxPlayer;
    //��С���߳���
    private const float MinPitch = 0.9f;
    //������߳���
    private const float MaxPitch = 1.1f;

    /// <summary>
    /// ��Ч���ź���,���ڲ���UI��Ч
    /// </summary>
    /// <param name="audioData"></param>
    public void PlaySFX(AudioData audioData)
    {
        sfxPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }
    /// <summary>
    /// ������Ų�ͬ��Ч����ͬ��Ч��ͬ���ߡ����������ظ����ŵ���Ч�����ӵ�������Ч
    /// </summary>
    /// <param name="audioData"></param>
    public void PlayerRandomSFX(AudioData audioData)
    {
        //������Ų�ͬ���ߵ���Ч
        sfxPlayer.pitch = Random.Range(MinPitch, MaxPitch);
        PlaySFX(audioData);
    }
    /// <summary>
    /// ������Ų�ͬ��Ч
    /// </summary>
    /// <param name="audioData">��Ƶ���ݼ���</param>
    public void PlayerRandomSFX(AudioData[] audioData)
    {
        //����Ƶ���ݼ��������һ����Ƶ���ϲ�����
        PlayerRandomSFX(audioData[Random.Range(0,audioData.Length)]);
    }
}
