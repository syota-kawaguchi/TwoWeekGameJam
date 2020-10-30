using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortrateCloset : FurnitureScript
{

    [SerializeField] private GameObject itemInCloset;
    private ItemController itemController;

    [SerializeField] private GameObject lockObject;

    private new void Start() {
        base.Start();
        if (itemInCloset) itemController = itemInCloset.GetComponent<ItemController>();
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        if (isLock) {
            actionText = MessageText.Open();
            keyCode = KeyCode.Space;

            if (GameTrigger.isPlayerHasDriver) {
                action = UnLock;
            }
            else {
                action = ShowLockedMessage;
            }

            return;
        }

        if (isDoorOpen) {
            actionText = MessageText.Close();
        }
        else {
            actionText = MessageText.Open();
        }

        action = OpenOrClose;
        keyCode = KeyCode.Space;
        return;
    }

    private void UnLock() {
        isLock = false;
        lockObject.SetActive(false);

        gameController.messageController.SetMessagePanel(MessageText.ComeOffScrew());
    }


}

//private void Update() {
//    if (isLock) {
//        if (!lockObject.activeSelf) lockObject.SetActive(true);
//    }
//    else {
//        if (lockObject.activeSelf) lockObject.SetActive(false);
//    }

//    if (isNear && Input.GetKeyDown(KeyCode.Space)) {
//        if (isLock) {
//            if (PlayerStatus.currentHasItem && PlayerStatus.currentHasItem.name == "Driver") {
//                gameController.messageController.SetMessagePanel(MessageText.OpenDoor());
//                isLock = false;

//            }
//            else {
//                gameController.messageController.SetMessagePanel(MessageText.LockedByScrew());
//            }
//        }
//        else {
//            if (isDoorOpen) {
//                animator.SetBool("Open", false);
//                isDoorOpen = false;

//                gameController.actionNavigationController.SetActionNavigation(KeyCode.Space, MessageText.Open());
//            }
//            else {
//                animator.SetBool("Open", true);
//                isDoorOpen = true;

//                if (itemInCloset) {
//                    gameController.actionNavigationController.SetActionNavigation(KeyCode.Space, MessageText.Check());
//                }
//                else {
//                    gameController.actionNavigationController.SetActionNavigation(KeyCode.Space, MessageText.Close());
//                }
//            }
//        }
//    }

//    if (isNear && itemInCloset && Input.GetMouseButtonDown(0) && isDoorOpen) {
//        itemInCloset = null;

//        itemController.GetItem();
//    }
//}