using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelfController : FurnitureScript
{
    [SerializeField] private GameObject books;
    private Rigidbody booksRigidbody;

    [SerializeField] private float addForcebooks = 0.1f;
    [SerializeField] private Vector3 addForceDirection;

    [SerializeField] private GameObject driverGrip;

    [SerializeField] private GameObject driver;

    //if this bool is true, do not raise an event
    [SerializeField] private bool isObject = false;

    private new void Start() {
        base.Start();

        booksRigidbody = books.GetComponent<Rigidbody>();

        driverGrip.SetActive(false);
    }

    private new void Update() {
        //Triggerがオンになったら本を落とし、ドライバーグリップをアクティブにする
        if (GameTrigger.isPlayerHasDriverTip && !GameTrigger.isFallBookFromShelf) {

            //メッセージが閉じていなかったら終了
            if (!gameController.messageController.isClose) return;

            GameTrigger.isFallBookFromShelf = true;

            booksRigidbody.AddForce(AddForceBooks());

            driverGrip.SetActive(true);

            Debug.Log("fall books");
        }
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        actionText = MessageText.Check();
        keyCode = KeyCode.Space;
        action = BookShelfAction;
    }

    protected override void ShowLockedMessage() {
        gameController.messageController.SetMessagePanel(MessageText.checkBookShelfText());
    }

    //Todo本が落ちる動作改良の余地あり
    public void FallBooksFromBookeShelf() { 
    }

    private Vector3 AddForceBooks() {
        return addForceDirection * addForcebooks;
    }

    private void BookShelfAction() {
        if (GameTrigger.isFallBookFromShelf && driverGrip.activeSelf == true && isObject) {
            if (GameTrigger.isPlayerHasDriverTip) {
                gameController.messageController.SetMessagePanel(MessageText.checkBookShelfText());

                driver.SetActive(true);

                var itemController = driver.GetComponent<ItemController>();
                itemController.GetItem();
            }
            else {
                gameController.messageController.SendMessage(MessageText.GetItemText("ドライバーグリップ"));

                PlayerStatus.currentHasItem = driverGrip;
            }
            driverGrip.SetActive(false);
        }
        else {
            gameController.messageController.SetMessagePanel(MessageText.checkBookShelfText());
        }
    }
}

//void Update() {
//    if (isNear && Input.GetKeyDown(KeyCode.Space)) {
//  
