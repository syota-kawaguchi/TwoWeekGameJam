using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLockUIController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    // Start is called before the first frame update
    void Start() {
        panel.SetActive(false);
    }

    public void padLockUIActive() {
        panel.SetActive(true);
    }

    public void padLockUIInactive() {
        panel.SetActive(false);
    }
}
