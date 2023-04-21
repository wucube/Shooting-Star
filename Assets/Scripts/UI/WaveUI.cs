using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 敌人波数UI
/// </summary>
public class WaveUI : MonoBehaviour
{
   private Text waveText;
   void Awake()
   {
      GetComponent<Canvas>().worldCamera = Camera.main;
      waveText = GetComponentInChildren<Text>();
   }
   private void OnEnable()
   {
      waveText.text = "- WAVE 0" + EnemyManager.Instance.WaveNumber + " -";
   }
}
