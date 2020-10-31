using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : FurnitureScript
{
    public Room doorType;

    private new void Start() {
        base.Start();
        audioClip = gameController.doorSource;
    }

    private new void Update() {
        CheckIsDoorUnlocked();
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        actionText = isDoorOpen ? MessageText.Close() : MessageText.Open();
        keyCode = KeyCode.Space;
        action = OpenOrClose;
    }

    protected override void OpenOrClose() {
        if (isLock) {
            if (IsPlayerHasKey()) {
                isLock = false;
                gameController.messageController.SetMessagePanel(MessageText.OpenDoor());
            }
            else {
                if (doorType == Room.roomD) {
                    if (PlayerStatus.currentHasItem && PlayerStatus.currentHasItem.name == "PadLock") {
                        gameController.messageController.SetMessagePanel(MessageText.RoomDLockText());
                        GameTrigger.isEnemyRoomLocked = true;
                    }
                    else {
                        gameController.messageController.SetMessagePanel(MessageText.RoomDText());
                    }
                }
                else {
                    gameController.messageController.SetMessagePanel(MessageText.Locked());
                }
            }
        }
        else {
            if (doorType == Room.entrance) {
                gameController.GameClear();
            }
            else {
                base.OpenOrClose();
            }
        }
    }

    private void CheckIsDoorUnlocked() {
        switch (doorType) {
            case Room.roomA:
                break;
            case Room.roomB:
                break;
            case Room.roomC:
                break;
            case Room.roomD:
                break;
            case Room.corridor:
                isLock = !GameTrigger.isCorridorDoorOpen;
                break;
            case Room.entrance:
                isLock = !GameTrigger.isEntranceDoorOpen;
                break;
            default:
            break;
        }
    }

    private bool IsPlayerHasKey() {

        if (PlayerStatus.currentHasItem == null) return false;

        switch (doorType) {
            case Room.roomA:
                return PlayerStatus.currentHasItem.name == "roomAKey";
            case Room.roomB:
                return PlayerStatus.currentHasItem.name == "roomBKey";
            case Room.roomC:
                return PlayerStatus.currentHasItem.name == "roomCKey";
            case Room.roomD:
                return PlayerStatus.currentHasItem.name == "roomDkey";
            default:
                return false;
        }
    }

    //敵が部屋に入るときにドアを開ける関数
    public void EnterEnemyInRoom() {
        if (isLock) isLock = false;

        animator.SetBool("Open", true);
    }

    //敵が部屋から出るときにドアを締める関数
    public void ExitEnemyFromRoom() {
        Debug.Log("ExitEnemy");
        isLock = false;
        animator.SetBool("Open", false);
    }
}
