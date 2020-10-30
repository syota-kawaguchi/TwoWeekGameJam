using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVStandDoor : FurnitureScript
{
    [SerializeField] private Transform door;

    private Vector3 doorClose = new Vector3(0, 0, 0);
    private Vector3 doorOpen  = new Vector3(0, 180, 0);

    private 
    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    new void Update() {
        
    }

    protected override void OpenOrClose() {
        if (isDoorOpen) {
            door.localEulerAngles = doorClose;
            isDoorOpen = false;
        }
        else {
            door.localEulerAngles = doorOpen;
            isDoorOpen = true;
        }
    }
}
