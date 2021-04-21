using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EightPuzzle : MonoBehaviour
{
    /**パズルの形
     * 1  2  3
     * 4  5  6
     * 7  8  9
     *   10 
     * **/
    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;

    [SerializeField] private GameObject leverSwitchObj;
    private LeverSwitch leverSwitch;

    [SerializeField] private GameObject eightPuzzleUI;
    [SerializeField] private GameObject eightPuzzleCameraObj;
    private Camera eightPuzzleCamera;

    //ピースを管理する配列
    [SerializeField] private GameObject[] pieces = new GameObject[10];
    //ピースの番号と場所を対応付ける配列
    [SerializeField] private Transform[] positions = new Transform[10];
    //空白のインデックス
    private int blankPieceIndex;
    //移動可能なピースのインデックスを管理する配列
    private List<int> enableMovePieces = new List<int>();

    private bool isMove = false;
    //動かしているオブジェクトのインデックス
    private int movingObjIndex;

    // Start is called before the first frame update
    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
        leverSwitch = leverSwitchObj.GetComponent<LeverSwitch>();

        for (int i = 0; i < pieces.Length; i++) {
            if (!pieces[i]) {
                blankPieceIndex = i;
                Debug.LogFormat("blankPIeceIndex : {0}", blankPieceIndex);
            }
        }

        UpdateEnableMoveObj(blankPieceIndex);

        if(!eightPuzzleCameraObj.activeSelf)eightPuzzleCameraObj.SetActive(true);
        eightPuzzleCamera = eightPuzzleCameraObj.GetComponent<Camera>();
        eightPuzzleCameraObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (eightPuzzleCameraObj.activeSelf) {

            if (Input.GetKeyDown(KeyCode.E)) Pop();


            if (Input.GetMouseButtonDown(0)) {

                if (isMove) return;

                Ray ray = eightPuzzleCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit)) {
                    var pieceObj = hit.collider.gameObject;
                    Debug.LogFormat("hit obj : {0}", pieceObj);

                    if (pieceObj.tag == "Piece") {
                        var pieceScript = pieceObj.GetComponent<Piece>();
                        MovePiece(pieceScript);
                    }
                }
            }
        }
    }

    private void MovePiece(Piece pieceScript) {
        int pieceIndex = pieceScript.GetPieceNum;
        //パズルのルール上移動不可または移動処理中のとき
        if (!enableMovePieces.Contains(pieceIndex) || isMove) return;

        //オブジェクトを画面上で移動させる処理
        Debug.LogFormat("movingObj : {0}",pieceIndex);
        isMove = true;

        pieces[pieceIndex].transform.position = positions[blankPieceIndex].position;

        //コード内での移動処理
        (pieces[pieceIndex], pieces[blankPieceIndex]) = (pieces[blankPieceIndex], pieces[pieceIndex]);
        pieceScript.SetPieceNum = blankPieceIndex;
        blankPieceIndex = pieceIndex;

        Debug.LogFormat("updated Array. Next blank is {0}", blankPieceIndex);

        //移動可能なオブジェクトの更新処理
        UpdateEnableMoveObj(blankPieceIndex);

        CheckAnswer();

        isMove = false;

    }

    private void UpdateEnableMoveObj(int _blankPieceObjIndex) {

        enableMovePieces = new List<int>();

        if (_blankPieceObjIndex == 9) {
            enableMovePieces.Add(7);
            return;
        }

        switch (_blankPieceObjIndex % 3) {
            case 0:
                enableMovePieces.Add(_blankPieceObjIndex + 1);
                break;
            case 1:
                enableMovePieces.Add(_blankPieceObjIndex + 1);
                enableMovePieces.Add(_blankPieceObjIndex - 1);
                break;
            case 2:
                enableMovePieces.Add(_blankPieceObjIndex - 1);
                break;
        }

        switch (_blankPieceObjIndex / 3) {
            case 0:
                enableMovePieces.Add(_blankPieceObjIndex + 3);
                break;
            case 1:
                enableMovePieces.Add(_blankPieceObjIndex + 3);
                enableMovePieces.Add(_blankPieceObjIndex - 3);
                break;
            case 2:
                enableMovePieces.Add(_blankPieceObjIndex - 3);
                break;
            default:
                break;
        }

        if (_blankPieceObjIndex == 7) {
            enableMovePieces.Add(9);
        }

        for (int i = 0; i < enableMovePieces.Count; i++) {
            Debug.LogFormat("enableMovePiece : {0}", enableMovePieces[i]);
        }
    }

    private void CheckAnswer() {
        for (int i = 0; i < 9; i++) {
            if (pieces[i] == null) return;

            if (int.Parse(pieces[i].name) != i) {
                return;
            }
        }
        Debug.Log("Finish");

        leverSwitch.UnlockPazzle();

        ClearEightPuzzle();
    }

    public void Push() {
        gameController.ClearUI();
        gameController.mainCamera.SetActive(false);
        gameController.eightPuzzleUI.SetActive(true);
        eightPuzzleCameraObj.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameTrigger.isEventScene = true;
    }

    private void ClearEightPuzzle() {
        eightPuzzleUI.SetActive(false);
        gameController.mainCamera.SetActive(true);
        eightPuzzleCameraObj.SetActive(false);

        gameController.messageController.SetMessagePanel(MessageText.HearUnlockSoundSomeWhere());

        StartCoroutine(gameController.waitCloseMessageUI(() => {
            GameTrigger.isEventScene = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameController.ChangePlayerUIActive(true);
        }));
    }

    private void Pop() {
        GameTrigger.isEventScene = false;
        Cursor.lockState = CursorLockMode.Locked;
        eightPuzzleUI.SetActive(false);
        gameController.ChangePlayerUIActive(true);
        gameController.eightPuzzleUI.SetActive(false);
        gameController.mainCamera.SetActive(true);
        eightPuzzleCameraObj.SetActive(false);
    }
}
