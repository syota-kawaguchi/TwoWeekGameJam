using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordController : MonoBehaviour
{
    [SerializeField] private GameObject gameControllerObj;
    GameController gameController;

    [SerializeField] private GameObject passwordUI;

    [SerializeField] private GameObject[] inputFieldObjects;
    private Text[] inputFields;
    //入力するフィールド配列の要素番号
    private int currentInput = 0;

    private string answer = "";

    private Action unlockedAction;

    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();

        inputFields = new Text[inputFieldObjects.Length];
        for (int i = 0; i < inputFields.Length; i++) {
            inputFields[i] = inputFieldObjects[i].GetComponent<Text>();
        }

        if (passwordUI.activeSelf) passwordUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (passwordUI.activeSelf) {
            if (Input.GetKeyDown(KeyCode.E)) {
                Pop();
            }
        }
    }

    public void ButtonPushed(int number) {

        if (currentInput >= inputFields.Length) return;

        inputFields[currentInput].text = number.ToString();
        currentInput++;

        if (currentInput >= 4) {
            CheckPassword();
        }
    }

    public void Push(Action _unlockedAction, string _answer) {
        gameController.ClearUI();
        Cursor.lockState = CursorLockMode.None;
        GameTrigger.isEventScene = true;

        this.unlockedAction = _unlockedAction;
        this.answer = _answer;

        passwordUI.SetActive(true);
    }

    private void Pop() {
        passwordUI.SetActive(false);
        StartCoroutine(gameController.waitCloseMessageUI(() =>
        {
            GameTrigger.isEventScene = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameController.ChangePlayerUIActive(true);
        }));
    }

    public void CheckPassword() {
        for (int i = 0; i < 4; i++) {
            if (inputFields[i].text != answer[i].ToString()) {
                StartCoroutine(gameController.DelaySecond(0.5f, () =>{
                    Refresh(isAll: true);
                }));
                return;
            }
        }

        Unlocked();
    }

    private void Refresh(bool isAll) {
        if (isAll) {
            for (int i = 0; i < inputFields.Length; i++) {
                inputFields[i].text = "*";
            }
            currentInput = 0;
        }
        else {
            currentInput--;
            inputFields[currentInput].text = "*";
        }
    }

    private void Unlocked() {
        unlockedAction();
        Pop();
    }
}
