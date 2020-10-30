using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSprite : ItemController
{
    [SerializeField] protected Sprite riddleSprite;

    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    public override void HandItemUIInfo(ref string actionText, ref KeyCode keycode, ref Action action) {
        actionText = MessageText.Check();
        keycode = KeyCode.Space;
        action = ItemAction;
    }

    protected override void ItemAction() {
        if (riddleSprite == null) {
            Console.WriteLine("break");
        }
        gameController.riddleUIController.SetRiddleImage(riddleSprite);
    }

    IEnumerator waitCloseMessageUI() {
        yield return new WaitUntil(() => gameController.messageController.isClose);

        gameController.riddleUIController.SetRiddleImage(riddleSprite);
    }
}
