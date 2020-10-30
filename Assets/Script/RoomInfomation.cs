using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfomation : MonoBehaviour
{
    [SerializeField] private string roomNumber;
    [SerializeField] private Transform roomCenterPosition;

    public string RoomNumber {
        get { return roomNumber;}
    }

    public Transform RoomCenterPosition {
        get { return roomCenterPosition;}
    }
}
