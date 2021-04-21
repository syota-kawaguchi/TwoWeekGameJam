using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverControlller : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;

    void Start() {
        if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;

        if (GameTrigger.gameOver) {
            gameOverPanel.SetActive(true);
            gameClearPanel.SetActive(false);
        }
        else {
            gameOverPanel.SetActive(false);
            gameClearPanel.SetActive(true);
        }
        GameTrigger.Refresh();
    }


    public void BackToTitle() {
        SceneManager.LoadScene("StartScene");
    }
}
