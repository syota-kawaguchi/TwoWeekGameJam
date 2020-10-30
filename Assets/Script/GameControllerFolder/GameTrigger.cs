using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    public static bool isCutScene            = false;

    public static bool isPlayerHasDriverTip  = false;

    public static bool isPlayerHasDriverGrip = false;

    public static bool isPlayerHasDriver     = false;

    public static bool isShakeFloor          = false;

    public static bool isFallBookFromShelf   = false;

    public static bool isEnemyInRoom2B       = false;

    public static bool isEventScene          = false; // Player disable move

    public static bool isEvent               = false; // Player disable move

    public static bool isCorridorDoorOpen    = false;

    public static bool isEntranceDoorOpen    = false;

    public static bool isEnemyRoomLocked     = false;

    public static bool gameOver              = false;

    public static bool playerDisableMove {
        get {
            return isEventScene || isEvent;
        }
    }

    public static void Refresh() {
        isCutScene = false;

        isPlayerHasDriverTip  = false;
        isPlayerHasDriverGrip = false;
        isPlayerHasDriver     = false;

        isShakeFloor          = false;
        isFallBookFromShelf   = false;
        isEnemyInRoom2B       = false;

        isEventScene          = false; // Player disable move
        isEvent               = false; // Player disable move
        isCorridorDoorOpen    = false;
        isEntranceDoorOpen    = false;
        isEnemyRoomLocked     = false;
        gameOver              = false;
    }
}
