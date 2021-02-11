
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //マウス感度
    public static float minMouseSensibility = 0.01f;
    public static float maxMouseSensibility = 1.0f;
    public static float defaultMouseSensibility = 0.5f;
    private static float mouseSensibility = defaultMouseSensibility;

    public static float getMouseSensibility { get { return mouseSensibility; }}
    public static float setMouseSensibility { set { mouseSensibility = value; }}

    //BGM音量
    public static float minBGMRatio = 0;
    public static float maxBGMRatio = 1.0f;
    public static float defaultBGMRatio = 0.6f;
    private static float bgmRatio = defaultBGMRatio;

    public static float getBGMRatio { get { return bgmRatio; } }
    public static float setBGMRatio { set { bgmRatio = value; } }
}
