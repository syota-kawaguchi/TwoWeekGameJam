using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScapeClosetController : FurnitureScript {

    private void Start() {
        base.Start();
        audioClip = gameController.wideDrawerSource;
    }

    private void Update() {
        base.Update();
    }
}
