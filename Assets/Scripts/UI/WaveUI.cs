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
      //设置画布组件的世界相机
      GetComponent<Canvas>().worldCamera = Camera.main;
      //取得文本对象组件，文本对象在波数UI中只有一个
      _waveText = GetComponentInChildren<Text>();
   }
   private void OnEnable()
   {
      //修改波数文本的值
      _waveText.text = "- WAVE 0" + EnemyManager.Instance.WaveNumber + " -";
   }
}
