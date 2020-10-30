using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject credit;

    public void OnTapStart() {
        SceneManager.LoadScene("MainScene");
        if (howToPlayPanel.activeSelf) howToPlayPanel.SetActive(false);
        if (credit.activeSelf) credit.SetActive(false);
    }

    public void OnTapHowToPlay() {
        howToPlayPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    public void OnTapBack() {
        startPanel.SetActive(true);
        credit.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void OnTapCredit() {
        startPanel.SetActive(false);
        credit.SetActive(true);
    }
}
