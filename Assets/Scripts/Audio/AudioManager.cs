using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    //��Դ�����������Ч������
    [SerializeField] private AudioSource sFXPlayer;
    //��С���߳���
    private const float MinPitch = 0.9f;
    //������߳���
    private const float MaxPitch = 1.1f;

    //��Ч���ź��� �����ڲ���UI��Ч
    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }
    //������Ų�ͬ��Ч����ͬ��Ч��ͬ���� �����ڻ������ظ����ŵ���Ч�����ӵ�������Ч
    public void PlayerRandomSFX(AudioData audioData)
    {
        //������Ų�ͬ���ߵ���Ч
        sFXPlayer.pitch = Random.Range(MinPitch, MaxPitch);
        PlaySFX(audioData);
    }
    //������Ų�ͬ��Ч�������أ�������Ƶ���ݼ���
    public void PlayerRandomSFX(AudioData[] audioData)
    {
        //����Ƶ���ݼ��������һ����Ƶ���ϲ�����
        PlayerRandomSFX(audioData[Random.Range(0,audioData.Length)]);
    }
}
