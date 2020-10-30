using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionPerception : MonoBehaviour
{
    [SerializeField] private MoveControllerClass onTriggerEnter = new MoveControllerClass();
    [SerializeField] private MoveControllerClass onTriggerStay  = new MoveControllerClass();
    [SerializeField] private MoveControllerClass onTriggerExit  = new MoveControllerClass();

    private void OnTriggerEnter(Collider other) {
        onTriggerEnter.Invoke(other);
    }
    private void OnTriggerStay(Collider other) {
        onTriggerStay.Invoke(other);
    }

    private void OnTriggerExit(Collider other) {
        onTriggerExit.Invoke(other);
    }

    [System.Serializable]
    public class MoveControllerClass : UnityEvent<Collider> { }
}
