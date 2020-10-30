using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConMonitor : FurnitureScript {

    [SerializeField] private GameObject airConditioner;
    private AirConditionerController airConController;

    [SerializeField] private GameObject airConControllerUI;

    [SerializeField] private GameObject textMeshObj;
    private TextMesh showtemperature;

    [SerializeField] private bool isRoomC = true;

    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
        airConController = airConditioner.GetComponent<AirConditionerController>();
        showtemperature = textMeshObj.GetComponent<TextMesh>();

        if (airConControllerUI.activeSelf) airConControllerUI.SetActive(false);
    }

    private void Update() {
        showtemperature.text = isRoomC ? gameController.roomCTemperature.ToString() : gameController.entranceRoomtemperature.ToString();

        if (airConControllerUI.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            PopUI(airConControllerUI);
        }

        if (airConControllerUI.activeSelf && Input.GetKeyDown(KeyCode.W)) {
            gameController.UpRoomTemperature(isRoomC: isRoomC);
        }

        if (airConControllerUI.activeSelf && Input.GetKeyDown(KeyCode.S)) {
            gameController.DownRoomTemperature(isRoomC: isRoomC);
        }
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
