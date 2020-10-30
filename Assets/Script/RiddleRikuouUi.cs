using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiddleRikuouUi : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text[] inputText;
    [SerializeField] private string password = "6553";

    private int currentInputCount = 0;

    private Action action;

    void Start() {
        ClearInputText();
        currentInputCount = 0;

        panel.SetActive(false);
    }

    void Update() {

    }

    private void ClearInputText() {
        for (int i = 0; i < 4; i++) {
            inputText[i].text = "*";
        }
    }

    public void ButtonPushed(int num) {
        inputText[currentInputCount].text = num.ToString();

        currentInputCount++;

        if (currentInputCount >= 4) {
            CheckAnswer();
        }
    }

    private void CheckAnswer() {
        for (int i = 0; i < 4; i++) {
            if (inputText[i].text != password[i].ToString()) {
                ClearInputText();
                currentInputCount = 0;
                return;
            }
        }

        action();
        Pop();
    }

    public void Push(Action action) {
        Cursor.lockState = CursorLockMode.None;
        GameTrigger.isEventScene = true;
        this.action = action;
        panel.SetActive(true);
        ClearInputText();
        currentInputCount = 0;
    }

    private void Pop() {
        Cursor.lockState = CursorLockMode.Locked;
        GameTrigger.isEventScene = false;
        panel.SetActive(false);
    }
}
