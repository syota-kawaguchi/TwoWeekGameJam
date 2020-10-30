using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    [SerializeField] private GameObject GameControllerObj;
    protected GameController gameController;

    private GameObject player;

    [SerializeField] private Transform parentObject;

    [SerializeField] private float itemPositionAdjust_x = 0;
    [SerializeField] private float itemPositionAdjust_y = 0.2f;
    [SerializeField] private float itemPositionAdjust_z = 0;

    [SerializeField] private string itemName = "";

    [HideInInspector] public bool isPlayerHaving = false;

    protected void Start() {
        gameController = GameControllerObj.GetComponent<GameController>();
        player = gameController.player;
    }

    void Update() {
        if (!isPlayerHaving && parentObject != null) {
            transform.position = FollowParentObject();
        }
    }

    public string GetName {
        get { return itemName; }
    }

    public virtual void HandItemUIInfo(ref string actionText, ref KeyCode keycode, ref Action action) {
        actionText = MessageText.PickUp();
        keycode = KeyCode.Mouse0;
        action = ItemAction;
    }

    protected virtual void ItemAction() {
        GetItem();
    }

    public void GetItem() {

        if (PlayerStatus.currentHasItem) {

            PlayerStatus.currentHasItem.transform.position = PutItemPosition();

            //プレイヤーが所持しているアイテムを手放してもプレイヤーに追従しないようにする処理
            var havingItem = PlayerStatus.currentHasItem.GetComponent<ItemController>();
            havingItem.RemoveItem();

            //プレイヤーが所持しているときはSetActiveをFalseにしているので手放したときにTrueにする。
            if (!PlayerStatus.currentHasItem.activeSelf) PlayerStatus.currentHasItem.SetActive(true);
        }

        if (gameController == null) {
            gameController = GameControllerObj.GetComponent<GameController>();
        }

        if (player == null) {
            player = gameController.player;
        }

        PlayerStatus.currentHasItem = this.gameObject;
        gameController.SetCurrentHasItem = itemName;

        gameController.messageController.SetMessagePanel(MessageText.GetItemText(itemName));

        ChangeItemTrigger();

        parentObject = player.transform;
        gameObject.SetActive(false);
    }

    public void RemoveItem() {
        parentObject = null;
    }

    private void ChangeItemTrigger() {

        if (GameTrigger.isPlayerHasDriver) GameTrigger.isPlayerHasDriver = false;

        switch (itemName) {
            case "ドライバーの先端":
                GameTrigger.isPlayerHasDriverTip = !GameTrigger.isPlayerHasDriverTip;
                break;
            case "ドライバーグリップ":
                GameTrigger.isPlayerHasDriverGrip = !GameTrigger.isPlayerHasDriverGrip;
                break;
            case "ドライバー":
                GameTrigger.isPlayerHasDriver = !GameTrigger.isPlayerHasDriver;
                break;
        }
    }

    private Vector3 FollowParentObject() {
        return new Vector3(
            parentObject.position.x + itemPositionAdjust_x,
            parentObject.position.y + itemPositionAdjust_y,
            parentObject.position.z + itemPositionAdjust_z
        );
    }
    private Vector3 PutItemPosition() {
        //問題があれば計算式
        if (player == null) {
            Console.WriteLine("player is null");
            return new Vector3(0, 0, 0);
        }

        var playerPos = player.transform.position;

        return new Vector3(playerPos.x, 0, playerPos.z);
    }
}
