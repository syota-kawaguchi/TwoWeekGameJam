using System;
using System.IO;
using System.Linq;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.UI;

/// <summary>
/// BGM切り替えサンプルのボタン
/// </summary>
public class BGMSwitchSampleButton : MonoBehaviour {

  //=================================================================================
  //再生、停止
  //=================================================================================
  
  /// <summary>
  /// BGM1を再生
  /// </summary>
  public void PlayBGM2() {
    PlayBGM(BGMPath.BATTLE27);
  }
  
  //BGMを再生
  private void PlayBGM(string bgmPath, float volumeRate = 1, bool allowsDuplicate = false) {
    Debug.Log(bgmPath + "再生開始");
    
    BGMManager.Instance.Play(bgmPath, volumeRate:volumeRate, allowsDuplicate:allowsDuplicate);
  }
  
  /// <summary>
  /// BGMを再生していたら停止する
  /// </summary>
  public void StopBGM() {
    Debug.Log("BGM停止");
    BGMManager.Instance.Stop();
  }
  
}