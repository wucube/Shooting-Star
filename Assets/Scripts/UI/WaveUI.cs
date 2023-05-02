using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
   private Text _waveText;

   void Awake()
   {
      //���û���������������
      GetComponent<Canvas>().worldCamera = Camera.main;
      //ȡ���ı�����������ı������ڲ���UI��ֻ��һ��
      _waveText = GetComponentInChildren<Text>();
   }
   private void OnEnable()
   {
      //�޸Ĳ����ı���ֵ
      _waveText.text = "- WAVE 0" + EnemyManager.Instance.WaveNumber + " -";
   }
}
