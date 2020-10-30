using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvelopScript : ItemController
{
    [SerializeField] private Sprite riddleSprite;

    private bool isNear;
    private bool isFirst = true;

    void Start() {
        base.Start();
    }

    void Update() {
    }

    public override void HandItemUIInfo(ref string actionText, ref KeyCode keycode, ref Action action) {
        actionText = MessageText.Check();
        keycode = KeyCode.Space;
        action = ItemAction;
    }

    protected override void ItemAction() {
        if (isFirst) {
            gameController.messageController.SetMessagePanel(MessageText.somethingInEnvelop());
            StartCoroutine(waitCloseMessageUI());
        }
        else {
            gameController.riddleUIController.SetRiddleImage(riddleSprite);
        }
    }

    IEnumerator waitCloseMessageUI() {
        yield return new WaitUntil(() => gameController.messageController.isClose);

        isFirst = false;
        gameController.riddleUIController.SetRiddleImage(riddleSprite);
    }
}
