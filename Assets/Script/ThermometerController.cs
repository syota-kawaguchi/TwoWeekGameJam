using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermometerController : FurnitureScript
{
    [SerializeField] private GameObject thermometerUI;
    [SerializeField] private Slider temperatureSlider;

    [SerializeField] private float defaultSliderValue = 0.788f;

    void Start() {

        base.Start();

        thermometerUI.SetActive(false);

        temperatureSlider.value = GetSliderValue(gameController.roomCTemperature);
    }

    void Update() {
        if (thermometerUI.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            PopUI(thermometerUI);
        }
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        actionText = MessageText.Check();
        keyCode = KeyCode.Space;
        action = PushThermometerScene;
    }

    public float GetSliderValue(float roomTem) {
        return defaultSliderValue + (roomTem - 27) * 0.03f;
    }

    private void PushThermometerScene() {

        if (gameController == null) {
            Console.WriteLine();
        }

        temperatureSlider.value = GetSliderValue(gameController.roomCTemperature);

        PushUI(thermometerUI);
    }
}
