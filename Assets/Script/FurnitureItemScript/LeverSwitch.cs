using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSwitch : FurnitureScript
{
    public bool isPasswordLock = true;  //
    private bool isFirstCheck = true;  //一回だけドアスイッチのMessageTextを表示させるBool
    private bool isShowMessage = false;

    private bool isChoice;
    private bool chose;
    private bool yes;

    [SerializeField] private Transform padLockTrans;
    [SerializeField] private Transform unlockedPadLockPuttingPosition;

    [SerializeField] private bool isDualLock = false;

    [SerializeField] private Room room = Room.roomA;

    [SerializeField] private Transform handle;
    [SerializeField] private Transform huta;


    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();

        if (padLockObj) padLockController = padLockObj.GetComponent<PadLockController>();
    }

    void Update() {
        if (isShowMessage) {
            if (gameController.messageController.isClose) {
                isShowMessage = false;
                PushPassWordPage();
            }
        }

        WaitForInput();
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        switch (lockType) {
            case LockType.password:

                keyCode = KeyCode.Space;

                if (isLock) {
                    actionText = MessageText.Open();
                    action = PushPassWordPage;
                }
                else {
                    //Open door switch
                    actionText = MessageText.Check();
                    action = CorridorDoorSwitch;
                }

                break;

            case LockType.padLock:

                keyCode = KeyCode.Space;

                if (isLock) {
                    actionText = MessageText.Open();
                    action = PushOpenPadLockScene;
                }
                else {
                    actionText = MessageText.Check();
                    action = CorridorDoorSwitch;
                }

                break;

            case LockType.puzzle:
                actionText = MessageText.Open();
                keyCode = KeyCode.Space;
                action = ShowLockedMessage;
                break;

            default:
                break;
        }
    }

    protected override void UnlockPadlock() {
        if (padLockTrans) {
            padLockTrans.position = unlockedPadLockPuttingPosition? unlockedPadLockPuttingPosition.position : new Vector3(0, 0, 0);
            padLockTrans.eulerAngles = new Vector3(0, 90, 0);

            if (unlockedPadLockPuttingPosition) padLockTrans.tag = "Item";
        }

        OpenHuta();

        Unlock(LockType.password);
    }

    private void Unlock(LockType _lockType) {
        if (isDualLock) {
            lockType = _lockType;
        }
        else {
            isLock = false;
        }
    }

    // done by passwordController.cs
    private void UnlockPassword() {
        isLock = false;
        isPasswordLock = false;
        gameController.messageController.SetMessagePanel(MessageText.checkDoorText(true));
    }

    private void Downhandle() {
        if (!handle) {
            Debug.Log("Set handle");
            return;
        }

        handle.eulerAngles = new Vector3(20, huta.eulerAngles.y, huta.eulerAngles.z);
    }

    private void OpenHuta() {
        if (!huta) {
            Debug.Log("Set huta");
            return;
        }
        var currentHutaAngle = huta.eulerAngles;

        huta.eulerAngles = new Vector3(currentHutaAngle.x, currentHutaAngle.y, 90);
    }

    //Pazzleが解除されたら実行する関数。主に外部で呼び出される
    public void UnlockPazzle() {
        Unlock(LockType.password);

        OpenHuta();
    }

    private void OpenEntrance() {

    }

    private void WaitForInput() {
        if (isChoice) {
            if (Input.GetKeyDown(KeyCode.Y)) {
                yes = true;
                chose = true;
            }
            else if (Input.GetKeyDown(KeyCode.N)) {
                yes = false;
                chose = true;
            }
        }
    }

    //廊下のドアのロックを管理する関数
    private void CorridorDoorSwitch() {
        //スイッチにロックが掛かっているとき
        if (isPasswordLock) {
            if (isFirstCheck) {
                gameController.messageController.SetMessagePanel(MessageText.CorridorDoorSwitchText());
                isShowMessage = true;
            }
            else {
                PushPassWordPage();
            }
        }
        else {
            gameController.messageController.SetMessagePanel(MessageText.DownLeverText());
            isChoice = true;
            StartCoroutine(WaitPushButton());
        }
    }

    private void PushPassWordPage() {
        gameController.passwordController.Push(() => UnlockPassword(), password);
    }

    private IEnumerator WaitPushButton() {

        yield return new WaitUntil(() => chose);

        if (yes) {
            // sound
            GameTrigger.isCorridorDoorOpen = true;
            Downhandle();
            gameController.messageController.SetMessagePanel(MessageText.ShouldHide());

            if (room == Room.entrance) GameTrigger.isEntranceDoorOpen = true;

            yield return new WaitUntil(() => gameController.messageController.isClose);

            if (room == Room.entrance) {
                //敵の部屋に鍵がかかってない場合は条件の処理でゲームオーバー
                if (!GameTrigger.isEnemyRoomLocked)gameController.enemyEnterProcess.OnStart(room);
            }
            else {
                gameController.countDownUIController.CountDownStart(initialValue: 7.0f, global::Room.roomC);
            }
        }
        else {
            gameController.messageController.SetMessagePanel(MessageText.Refrain());
            gameController.ClearUI();
        }

        chose = false;
        isChoice = false;
    }
}
