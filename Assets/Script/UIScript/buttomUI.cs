using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttomUI : MonoBehaviour
{
    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;

    [SerializeField] private GameObject panel;

    private int[] answer = { 1, 2, 2, 3, 1 };
    private int[] playerPushButtonNam = new int[5];

    private int pushCount = 0;

    private Action action;

    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
        panel.SetActive(false);
    }

    private void Update() {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            Pop();
        }
    }

    public void PushButton(int pushButtonNam) {
        //sound

        if (!GameTrigger.isEnemyRoomLocked) {
            // GameOverProcess
        }

        playerPushButtonNam[pushCount] = pushButtonNam;

        pushCount++;

        if(pushCount >= 5) {
            CheckAnswer();
        }

    }

    private void CheckAnswer() {
        for(int i = 0; i < 5; i++) {
            if (playerPushButtonNam[i] != answer[i]) {
                pushCount = 0;

                //Sound

                return;
            }

            action();
            Pop();
        }
    }

    public void Push(Action action) {
        Cursor.lockState = CursorLockMode.None;
        GameTrigger.isEventScene = true;
        this.action = action;
        panel.SetActive(true);
        ClearPushButton();
        pushCount = 0;
    }

    public void Exit() {
        Pop();
    }

    private void Pop() {
        Cursor.lockState = CursorLockMode.Locked;
        GameTrigger.isEventScene = false;
        panel.SetActive(false);
    }

    private void ClearPushButton() {
    }
}
