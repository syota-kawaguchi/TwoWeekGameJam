using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerDoorController : FurnitureScript
{
    [SerializeField] private GameObject player;

    private bool isPlayerHidden = false;

    //プレイヤーが隠れる場所
    private Vector3 playerHidePosition;
    //プレイヤーが隠れる前の場所を保持
    private Vector3 keepPlayerPosition;

    [SerializeField] private GameObject hideCamera;

    private new void Start() {
        base.Start();
        playerHidePosition = new Vector3(transform.position.x, 1, transform.position.z);
        hideCamera.SetActive(false);
    }

    private new void Update() {
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        actionText = isPlayerHidden ? MessageText.Exit() : MessageText.Hide();
        keyCode = KeyCode.Space;
        action = HideLocker;
    }

    private void HideLocker() {
        if (isPlayerHidden) {
            player.transform.position = keepPlayerPosition;

            hideCamera.SetActive(false);
            PlayerStatus.isPlayerHide = false;
            isPlayerHidden = false;

        }
        else {
            keepPlayerPosition = player.transform.position;
            player.transform.position = playerHidePosition;

            hideCamera.SetActive(true);
            PlayerStatus.isPlayerHide = true;
            isPlayerHidden = true;
            Debug.Log("hideLockerMethod");
        }
    }
}
