using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreasureBox : FurnitureScript
{
    [SerializeField] private GameObject top;

    public bool isPasswordLock   = true;  //
    private bool isFirstCheck    = true;  //一回だけドアスイッチのMessageTextを表示させるBool
    private bool isShowMessage   = false;

    private bool isChoice;
    private bool chose;
    private bool yes;

    [SerializeField] private Transform padLockTrans;

    [SerializeField] private GameObject riddle_RikuoObj;
    private RiddleRikuouUi riddleRikuo;

    [SerializeField] private GameObject riddle;

    [SerializeField] private GameObject buttonUiObj;
    private buttomUI buttonUI;

    private bool isRiddleClear = false;

    [SerializeField] private GameObject itemInBox;
    private ItemController itemController;

    [SerializeField] private string password_s = "";

    [SerializeField] private bool isDualLock = false;

    [SerializeField] private Room room = Room.roomA;


    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();

        if (padLockObj) padLockController = padLockObj.GetComponent<PadLockController>();

        if (riddle_RikuoObj) riddleRikuo = riddle_RikuoObj.GetComponent<RiddleRikuouUi>();

        if (buttonUiObj) buttonUI = buttonUiObj.GetComponent<buttomUI>();

        if (itemInBox) itemController = itemInBox.GetComponent<ItemController>();
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
            case LockType.puzzle:

                actionText = MessageText.Open();
                keyCode = KeyCode.Space;

                if (isLock) {
                    messageText = MessageText.Locked();
                    action  = base.ShowLockedMessage; 
                }
                else {
                    action = CorridorDoorSwitch;
                }

                break;

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

                if (isLock) {
                    actionText = MessageText.Open();
                    keyCode = KeyCode.Space;
                    action = PushOpenPadLockScene;
                }
                else {
                    actionText = MessageText.Check();
                    keyCode = KeyCode.Space;
                    action = GetItemInBox;
                }

                break;

            case LockType.screw:
                break;

            case LockType.riddle:

                //Todo
                if (true) { }

                break;
        }
    }

    protected override void UnlockPadlock() {
        if (padLockTrans) {
            padLockTrans.position = new Vector3(-13.8f, 0.95f, 9);
            padLockTrans.eulerAngles = new Vector3(0, 90, 0);
        }

        OpenTop();

        if (isDualLock) {
            lockType = LockType.password;
        }
        else {
            isLock = false;
        }
    }

    // done by passwordController.cs
    private void UnlockPassword() {
        isPasswordLock = false;
        gameController.messageController.SetMessagePanel(MessageText.checkDoorText(true));
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

    private void OpenTop() {
        if (!isLock) {
            top.transform.eulerAngles = new Vector3(-90, 90, 0);
        }
    }

    //宝箱の中にアイテムがあればそれを取得する関数
    private void GetItemInBox() {
        if (itemInBox) {
            itemController.GetItem();
            itemInBox = null;
        }
        else {
            gameController.messageController.SetMessagePanel(MessageText.BoxIsEmpty());
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
        gameController.passwordController.Push(() => UnlockPassword(), password_s);
    }

    private IEnumerator WaitPushButton(){

        yield return new WaitUntil(() => chose);

        if (yes) {
            // sound
            GameTrigger.isCorridorDoorOpen = true;
            gameController.messageController.SetMessagePanel(MessageText.ShouldHide());

            yield return new WaitUntil(() => gameController.messageController.isClose);

            gameController.countDownUIController.CountDownStart(initialValue: 7.0f, global::Room.roomC);
        }
        else {
            gameController.messageController.SetMessagePanel(MessageText.Refrain());
            gameController.ClearUI();
        }

        chose = false;
        isChoice = false;
    }
}
