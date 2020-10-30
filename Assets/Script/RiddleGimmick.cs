using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RiddleGimmick : ShowSprite
{
    [SerializeField] private GameObject imageBeforeSprite;
    [SerializeField] private GameObject imageAfterSprite;

    private Sprite activeSprite;

    private bool onChanged = false;

    //Riddleを切り替える温度
    float changeRiddleTemperature = 20;

    public enum GimmickType {
        none,
        eraser,
        cool
    }

    public GimmickType gimmickType = GimmickType.none;

    new void Start() {
        base.Start();

        activeSprite = imageBeforeSprite.GetComponent<SpriteRenderer>().sprite;

        riddleSprite = activeSprite;
        imageAfterSprite.SetActive(false);
    }


    //this method is run at EntranceRoomiu
    protected override void ItemAction() {
        ChangeShowRiddleGimmick();
    }

    //表示する謎を切り替える処理
    private void ChangeShowRiddleGimmick() {
        switch (gimmickType) {
            case GimmickType.cool:
                float _temperature = gameController.entranceRoomtemperature;

                if (_temperature == changeRiddleTemperature && !onChanged) {
                    ChangeShowRiddle();
                }
                else {
                    base.ItemAction();
                }
                break;

            case GimmickType.eraser:

                if (PlayerStatus.currentHasItem && PlayerStatus.currentHasItem.name == "Eraser" && !onChanged) {
                    gameController.messageController.SetMessagePanel(MessageText.TryErase());

                    ChangeShowRiddle();
                }
                else {
                    base.ItemAction();
                }
                break;  

            default:
                break;
        }

    }

    private void ChangeShowRiddle() {
        onChanged = true;
        activeSprite = imageAfterSprite.GetComponent<SpriteRenderer>().sprite;

        imageBeforeSprite.SetActive(false);
        imageAfterSprite.SetActive(true);

        riddleSprite = activeSprite;
    }
}
