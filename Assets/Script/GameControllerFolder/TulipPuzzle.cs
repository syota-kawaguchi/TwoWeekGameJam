using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TulipPuzzle : MonoBehaviour {
    //Ans Left : red,  Center : White, Right : Yellow

    [SerializeField] private GameObject gameControlleObj;
    private GameController gameController;

    private GameObject[] tulipObjects = new GameObject[3];

    [SerializeField] private GameObject[] defaultObjects; // 1: leftObject, 2: center, 3: right

    [SerializeField] private Vector3[] defaultPosition = new Vector3[3];

    [SerializeField] private string[] answer = { "赤いチューリップ", "白いチューリップ", "黄色いチューリップ" };

    [SerializeField] private GameObject unlockObj;
    private LeverSwitch leverSwitch;

    private bool isNear;

    private enum Position {
        Left   = 0,
        Center = 1,
        Right  = 2
    }

    Position position;

    void Start()
    {
        for (int i = 0; i < tulipObjects.Length; i++) {
            tulipObjects[i] = defaultObjects[i];
            defaultPosition[i] = defaultObjects[i].transform.position;
        }

        gameController = gameControlleObj.GetComponent<GameController>();
        leverSwitch = unlockObj.GetComponent<LeverSwitch>();
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.Space)) {
            if (PlayerStatus.currentHasItem) {
                if (tulipObjects[(int)position]) {
                    ExchangeItem();
                }
                else {
                    PutItem();
                }
            }
            else {
                if (tulipObjects[(int)position]){
                    GetItem();
                }
            }
            for (int i = 0; i < tulipObjects.Length; i++) {
                var name = tulipObjects[i] == null ? "null" : tulipObjects[i].name;
                print(i.ToString() + " : " + name);
            }

            CheckAnswer();
        }
    }

    private void ExchangeItem() {
        var relayObject = PlayerStatus.currentHasItem;
        PlayerStatus.currentHasItem = tulipObjects[(int)position];
        tulipObjects[(int)position].SetActive(false);
        tulipObjects[(int)position] = relayObject;

        SetPosition();

        gameController.messageController.SetMessagePanel(MessageText.ExchangePlayerItem(PlayerStatus.currentHasItem, tulipObjects[(int)position]));
    }

    private void PutItem() {
        tulipObjects[(int)position] = PlayerStatus.currentHasItem;

        SetPosition();

        gameController.messageController.SetMessagePanel(MessageText.PutPlayerItem(PlayerStatus.currentHasItem));

        PlayerStatus.currentHasItem = null;
    }

    private void GetItem() {
        PlayerStatus.currentHasItem = tulipObjects[(int)position];
        tulipObjects[(int)position].SetActive(false);
        tulipObjects[(int)position] = null;

        gameController.messageController.SetMessagePanel(MessageText.GetItemText(PlayerStatus.currentHasItem.name));
    }

    private void CheckAnswer() {
        for (int i = 0; i < tulipObjects.Length; i++) {
            if (tulipObjects[i] == null) {
                Debug.Log(tulipObjects[i]);
                return;
            }
            if (tulipObjects[i].name != answer[i]) {
                Debug.Log("name : " + tulipObjects[i].name + "ans : " + answer[i]);
                return;
            }
        }

        //解除処理
        Debug.Log("OK");
        leverSwitch.UnlockPazzle();


    }

    public void SetPosition() {
        if (!tulipObjects[(int)position].activeSelf) tulipObjects[(int)position].SetActive(true);

        tulipObjects[(int)position].transform.position = defaultPosition[(int)position];
    }

    public void EnterLeft(Collider other) {
        position = Position.Left;

        EnterPlayer(other);
    }

    public void EnterCenter(Collider other) {
        position = Position.Center;

        EnterPlayer(other);
    }

    public void EnterRight(Collider other) {
        position = Position.Right;

        EnterPlayer(other);
    }

    private void EnterPlayer(Collider other) {

        if (other.tag != "Player") return;

        gameController.actionNavigationController.SetActionNavigation(KeyCode.Mouse0, MessageText.PickUp());
        isNear = true;
    }

    public void ExitPlayer(Collider other) {
        if (other.tag == "Player") {
            isNear = false;

            gameController.actionNavigationController.ClearActionNavigation();
        }
    }
}
