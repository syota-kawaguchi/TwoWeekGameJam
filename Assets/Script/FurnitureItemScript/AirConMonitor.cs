using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConMonitor : FurnitureScript {

    [SerializeField] private GameObject airConditioner;
    private AirConditionerController airConController;

    [SerializeField] private GameObject airConControllerUI;

    [SerializeField] private float defaultRoomTemperature = 27.0f;
    private float currentRoomTemperature;
    private float maxRoomTemperature = 30.0f;
    private float minRoomTemperature = 10.0f;

    [SerializeField] private GameObject textMeshObj;
    private TextMesh showtemperature;

    new void Start() {
        base.Start();
        airConController = airConditioner.GetComponent<AirConditionerController>();
        showtemperature = textMeshObj.GetComponent<TextMesh>();
        currentRoomTemperature = defaultRoomTemperature;

        if (airConControllerUI.activeSelf) airConControllerUI.SetActive(false);
    }

    public float GetRoomtemperature {
        get { return currentRoomTemperature; }
    }

    new void Update() {
        showtemperature.text = currentRoomTemperature.ToString();

        if (airConControllerUI.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            PopUI(airConControllerUI);
        }

        if (airConControllerUI.activeSelf && Input.GetKeyDown(KeyCode.W)) {
            UpRoomTemperature();
        }

        if (airConControllerUI.activeSelf && Input.GetKeyDown(KeyCode.S)) {
            DownRoomTemperature();
        }
    }

    public void UpRoomTemperature() {
        if (currentRoomTemperature >= 30) return;

        currentRoomTemperature += 1;
    }

    public void DownRoomTemperature() {
        if (currentRoomTemperature <= 10) return;

        currentRoomTemperature -= 1;
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        actionText = MessageText.Check();
        keyCode = KeyCode.Space;
        action = ActiveMonitorUI;
    }

    private void ActiveMonitorUI() {
        PushUI(airConControllerUI);
    }
}
