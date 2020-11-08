using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edit : MonoBehaviour
{
    /*
        設定項目を扱うスクリプト
     */

    private GameController gameController;

    [Space(2)]
    private GameObject settingUI;

    [Space(2)]
    //マウス感度割合
    [SerializeField] private GameObject mouseSensitivilitySliderObg;
    private Slider mouseSensitivilitySlider;

    [SerializeField] private float minMouseSensibility = 0.01f;
    [SerializeField] private float maxMouseSensibility = 1.0f;
    [SerializeField] private float defaultMouseSensibility = 0.5f;

    private void Start() {
        gameController = GetComponent<GameController>();

        mouseSensitivilitySlider = mouseSensitivilitySliderObg.GetComponent<Slider>();
        SetMouseSensivility();
    }

    private void SetMouseSensivility() {
        mouseSensitivilitySlider.maxValue = maxMouseSensibility;
        mouseSensitivilitySlider.minValue = minMouseSensibility;
        mouseSensitivilitySlider.value = defaultMouseSensibility;
    }
    public float MouseSensitivilityRatio {
        get { return mouseSensitivilitySlider.value; }
    }
}
