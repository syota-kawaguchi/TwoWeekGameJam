using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Key {
    Space,
    MouseLeft,
    W,
    S
}

public class ActionNavigationController : MonoBehaviour
{
    [Header("ButtonSprite")]
    [SerializeField] private Sprite spaceButton;
    [SerializeField] private Sprite mouseLeft;

    [Header("This UI")]
    [SerializeField] private GameObject panel;

    [SerializeField] private GameObject actionImageObj;
    private Image actionImage;

    [SerializeField] private GameObject actiontextObj;
    private Text actiontext;

    void Start() {
        actionImage = actionImageObj.GetComponent<Image>();
        actiontext  = actiontextObj.GetComponentInChildren<Text>();
        panel.SetActive(false);
    }

    void Update() {
    }

    public void SetActionNavigation(KeyCode keyCode, string actionExplanation) {
        actiontext.text = actionExplanation;

        switch (keyCode) {
            case KeyCode.Space:
                actionImage.sprite = spaceButton;
                break;
            case KeyCode.Mouse0:
                actionImage.sprite = mouseLeft;
                break;
            default:
                actionImage.sprite = spaceButton;
                break;
        }

        panel.SetActive(true);
    }

    public void ClearActionNavigation() {
        actiontext.text = "";
        panel.SetActive(false);
    }

    public void Active(bool b) {
        panel.SetActive(b);
    }
}
