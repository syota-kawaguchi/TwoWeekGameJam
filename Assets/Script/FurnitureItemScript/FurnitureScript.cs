using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FurnitureScript : MonoBehaviour
{
    public  bool isLock = true;
    protected bool isDoorOpen = false;

    protected Animator animator;

    [SerializeField] protected GameObject gameControllerObj;
    protected GameController gameController;

    [SerializeField] protected GameObject padLockObj;
    protected PadLockController padLockController;

    [SerializeField] protected string password = "0618";

    [SerializeField] protected GameObject fastener;

    public enum LockType {
        unLock,
        padLock,
        password,
        screw,
        riddle,
        puzzle
    }

    public LockType lockType;

    protected void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
        if (!animator) animator = GetComponent<Animator>();
        if (padLockObj) padLockController = padLockObj.GetComponent<PadLockController>();
    }

    protected virtual void OpenOrClose() {
        if (!animator) {
            Debug.Log(gameObject.name +  " : animator is null");
            return;
        }

        if (isDoorOpen) {
            animator.SetBool("Open", false);
            isDoorOpen = false;
        }
        else {
            animator.SetBool("Open", true);
            isDoorOpen = true;
        }
    }

    protected virtual void ShowLockedMessage() {
        if (lockType == LockType.screw) {
            gameController.messageController.SetMessagePanel(MessageText.LockedByScrew());
        }
        else {
            gameController.messageController.SetMessagePanel(MessageText.Locked());
        }
    }

    public virtual void handFurnitureUIInfo(ref string messageText, ref string actionText, ref KeyCode keyCode, ref Action action) {
        if (isLock) {
            actionText = MessageText.Open();
            keyCode = KeyCode.Space;
            action = PushOpenPadLockScene;
            return;
        }

        if (isDoorOpen) {
            actionText = MessageText.Close();
        }
        else {
            actionText = MessageText.Open();
        }

        action = OpenOrClose;
        keyCode = KeyCode.Space;
    }

    protected void PushOpenPadLockScene() {
        Time.timeScale = 0;

        GameTrigger.isEventScene = true;
        Cursor.lockState = CursorLockMode.None;

        gameController.actionNavigationController.ClearActionNavigation();
        gameController.ChangePlayerUIActive(false);

        gameController.mainCamera.SetActive(false);
        gameController.padLockCamera.SetActive(true);
        gameController.padLockUIController.padLockUIActive();

        padLockController.Push(password, () => UnlockPadlock());
    }

    protected virtual void UnlockPadlock() {
        isLock = false;
        if (fastener) fastener.transform.Rotate(0, 180, 0);
    }

    protected virtual void PopUI(GameObject ui) {

        if (ui == null) {
            Console.WriteLine(ui);
        }
        else if (gameController == null) {
            Console.WriteLine(gameController);
        }

        if (ui.activeSelf) ui.SetActive(false);
        if (GameTrigger.isEventScene) GameTrigger.isEventScene = false;
        gameController.ChangePlayerUIActive(true);
    }

    protected virtual void PushUI(GameObject ui) {
        if (ui == null) {
            Console.WriteLine(ui);
        }
        else if (gameController == null) {
            Console.WriteLine(gameController);
        }
        ui.SetActive(true);
        GameTrigger.isEventScene = true;
        if (gameController.IsPlayerUIActive) gameController.ChangePlayerUIActive(false);
    }

    private void PushOpenElectronicLockScene() {
        Time.timeScale = 0;
    }

    protected string PasswordToString(int[] _password) {

        string password_str = "";

        for (int i = 0; i < _password.Length; i++) {
            password_str = _password[i].ToString();
        }

        return password_str;
    }

    protected void Update() { }

}
