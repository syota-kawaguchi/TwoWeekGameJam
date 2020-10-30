using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUIController : MonoBehaviour
{
    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;

    [SerializeField] private GameObject countdownPanel;

    [SerializeField] private Text counttext;
    private float initialValue;

    private Action inTimeAction;

    private bool isCountDown = false;

    private Room room;

    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
        countdownPanel.SetActive(false);
    }

    void Update() {
        if (isCountDown) {
            counttext.text = initialValue.ToString("F2");

            initialValue -= initialValue > 0 ? Time.deltaTime : 0;

            if (initialValue <= 0) {
                isCountDown = false;
                gameController.GameOver();
            }

            if (PlayerStatus.isPlayerHide) {
                CountDownEnd();
            }
        }
    }

    public void CountDownStart(float initialValue, Room room) { //initialValue：カウントダウン秒数
        this.initialValue   = initialValue;
        this.room = room;
        //this.inTimeAction   = inTimeAction;
        countdownPanel.SetActive(true);

        isCountDown = true;
    }

    private void CountDownEnd() {
        isCountDown = false;
        countdownPanel.SetActive(false);
        gameController.enemyEnterProcess.OnStart(_room: room);
    }
}
