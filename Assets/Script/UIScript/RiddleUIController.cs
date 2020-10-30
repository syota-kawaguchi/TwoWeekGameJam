using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RiddleUIController : MonoBehaviour
{
    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;

    [SerializeField] private GameObject riddleImageObj;
    [SerializeField] private GameObject panel;
    private Image riddleImage;

    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
        riddleImage = riddleImageObj.GetComponent<Image>();
        panel.SetActive(false);
    }

    void Update() {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            Exit();
        }
    }

    public void SetRiddleImage(Sprite sprite) {
        if(!panel.activeSelf)panel.SetActive(true);

        if (sprite == null) {
            Console.WriteLine("break");
        }

        gameController.ClearUI();
        gameController.ChangePlayerUIActive(false);

        riddleImage.sprite = sprite;
        GameTrigger.isEventScene = true;
    }

    private void Exit() {
        panel.SetActive(false);
        riddleImage.sprite = null;
        gameController.ChangePlayerUIActive(true);
        GameTrigger.isEventScene = false;
    }

    public void OnPushedExit() {
        Exit();
    }

}
