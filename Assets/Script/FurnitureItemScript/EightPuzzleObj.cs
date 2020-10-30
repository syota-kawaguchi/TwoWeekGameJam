using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightPuzzleObj : FurnitureScript
{

    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    public override void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        actionText = MessageText.Check();
        keyCode = KeyCode.Space;
        action = PushEightPuzzleScene;
    }

    private void PushEightPuzzleScene() {
        gameController.eightPuzzle.Push();
    }
}
