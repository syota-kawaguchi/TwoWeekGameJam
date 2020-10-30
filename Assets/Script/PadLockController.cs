using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLockController : MonoBehaviour
{
    private int dialDigits = 4;
    [SerializeField]
    private GameObject[] dial;
    private int[] dialNum = new int[4];

    private Vector3[] defaultDialRotation = new Vector3[4];

    private int[] password;

    private Vector3 rotateDial = new Vector3(0, 0, 36);

    private Action unlockedAction;

    private bool isPadLockScene = false;

    private bool isProcessing = false;

    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;


    void Start()
    {
        gameController = gameControllerObj.GetComponent<GameController>();

        for (int i = 0; i < dialNum.Length; i++) {
            dialNum[i] = 0;

            defaultDialRotation[i] = dial[i].transform.eulerAngles;
        }
    }

    private void Update() {
        if (isPadLockScene && Input.GetKeyDown(KeyCode.E)) {
            Exit();
        }
    }

    public void OnClickTopDial() {
        int top = 0;

        RotateDial(top);
    }

    public void OnClickMiddleTopDial() {
        int middleTop = 1;

        RotateDial(middleTop);
    }

    public void OnClickMiddleBottomDial() {
        int middleBottom = 2;

        RotateDial(middleBottom);
    }

    public void OnClickBottomDial() {
        int bottom = 3;

        RotateDial(bottom);
    }

    public void OnClickExit() {
        Exit();
    }

    private void RotateDial(int num) {

        if (isProcessing) return;

        isProcessing = true;

        dial[num].transform.Rotate(rotateDial);
        if (dialNum[num] >= 9) {
            dialNum[num] = 0;
        }
        else {
            dialNum[num] += 1;
        }

        Debug.Log(dialNum[0].ToString() + dialNum[1].ToString() + dialNum[2].ToString() + dialNum[3].ToString());

        checkDialNumtoPassWord();
    }

    private void checkDialNumtoPassWord() {
        for (int i = 0; i < dialDigits; i++) {
            if (dialNum[i] != password[i]) {
                isProcessing = false;
                return;
            }
        }

        isProcessing = false;

        Open();
    }

    private void Open() {
        Debug.Log("Done");

        unlockedAction();

        Exit();
    }

    public void Push(string _password, Action action) {
        unlockedAction = action;
        this.password = PasswordToInt(_password);

        Debug.LogFormat("{0}{1}{2}{3}", password[0], password[1], password[2], password[3]);

        isPadLockScene = true;
    }

    private int[] PasswordToInt(string _password_str) {
        int[] result = new int[_password_str.Length];

        for (int i = 0; i < _password_str.Length; i++) {
            result[i] = int.Parse(_password_str[i].ToString());
        }

        return result;
    }

    public void Exit() {
        gameController.padLockUIController.padLockUIInactive();
        gameController.padLockCamera.SetActive(false);
        gameController.mainCamera.SetActive(true);
        gameController.ChangePlayerUIActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        GameTrigger.isEventScene = false;
        isPadLockScene = false;
        Time.timeScale = 1.0f;

        for (int i = 0; i < dialNum.Length; i++) {
            dialNum[i] = 0;
            dial[i].transform.eulerAngles = defaultDialRotation[i];
        }
    }

}
