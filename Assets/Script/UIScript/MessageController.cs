using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MessageController : MonoBehaviour
{
    [SerializeField] private GameObject gameContollerObj;
    private GameController gameController;

    private string getshowMessage;

    private Text showMessageText;

    [SerializeField] private GameObject messagePanel;

    private string splitString = "<>";
    private string[] splitMessage;
    private int messageNum;
    [SerializeField] private float textSpeed = 0.05f;
    private float elapsedTime = 0f;
    private int currentTextNum;
    private bool isShowOneMessage = false;
    private bool isEndMessage     = false;

    [SerializeField] private Image clickIcon;
    [SerializeField] private float clickIconFlashTime = 0.2f;

    public bool isClose {
        get { return isEndMessage; }
    }

    void Start() {
        gameController = gameContollerObj.GetComponent<GameController>();
        clickIcon.enabled = false;
        showMessageText = GetComponentInChildren<Text>();
        showMessageText.text = "";
        messagePanel.SetActive(false);
    }

    void Update() {
        ShowMessageLogic();
    }

    void ShowMessageLogic() {
        if (isEndMessage || getshowMessage == null || getshowMessage == "" || getshowMessage == "hogehoge") {
            return;
        };

        if (!isShowOneMessage) {
            if (elapsedTime > textSpeed) {
                showMessageText.text += splitMessage[messageNum][currentTextNum];

                currentTextNum++;
                elapsedTime = 0f;

                if (currentTextNum >= splitMessage[messageNum].Length) {
                    isShowOneMessage = true;
                }
            }
            elapsedTime += Time.deltaTime;

            if (Input.GetMouseButtonDown(0)) {
                showMessageText.text += splitMessage[messageNum].Substring(currentTextNum);
                isShowOneMessage = true;
            }
        }
        else {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= clickIconFlashTime) {
                clickIcon.enabled = !clickIcon.enabled;
                elapsedTime = 0;
            }

            if (Input.GetMouseButtonDown(0)) {
                currentTextNum = 0;
                messageNum++;
                showMessageText.text = "";
                clickIcon.enabled = false;
                elapsedTime = 0f;
                isShowOneMessage = false;

                if (messageNum >= splitMessage.Length) {
                    isEndMessage  = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    gameController.ChangePlayerUIActive(true);
                }
            }
        }
    }

    private void SetMessage(string message) {
        getshowMessage = message;

        splitMessage = Regex.Split(getshowMessage, @"\s*" + splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        currentTextNum = 0;
        messageNum = 0;
        showMessageText.text = "";
        isShowOneMessage = false;
        isEndMessage     = false;
    }

    public void SetMessagePanel(string message) {
        SetMessage(message);
        messagePanel.SetActive(true);
        gameController.ChangePlayerUIActive(false);
    }

    public void Active(bool b) {
        messagePanel.SetActive(b);
    }
}
