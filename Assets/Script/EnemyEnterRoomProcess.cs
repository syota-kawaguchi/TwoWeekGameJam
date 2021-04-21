using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum Room { 
     roomA,
     roomB,
     roomC,
     roomD,
     corridor,
     entrance
}
public class EnemyEnterRoomProcess : MonoBehaviour {
    private GameController gameController;

    [SerializeField] private Transform player;

    private GameObject enemy;
    private EnemyController enemyController;
    private Animator enemyAnimator;

    private DoorController doorController;

    private bool searchPlayerFlag = false;

    [Header("RoomA")]
    [SerializeField] private GameObject doorA;

    [SerializeField] private GameObject EnterRoomATimeLineObj;
    private PlayableDirector enterRoomATimeLine;

    [Header("RoomC")]
    [SerializeField] private GameObject doorC;

    [SerializeField] private GameObject EnterRoomCTimeLineObj;
    private PlayableDirector enterRoomCTimeLine;

    [Header("EntranceDoor")]
    [SerializeField] private GameObject enterEntranceTimeLineObj;
    private PlayableDirector enterRoomEntranceRoomTimeLine;

    private bool onStartAnimate = false;
    private float animationTime = 0f;

    private Room room;



    void Start() {
        gameController = GetComponent<GameController>();

        enemy = gameController.enemy;
        enemyController = enemy.GetComponent<EnemyController>();
        enemyAnimator = enemy.GetComponent<Animator>();

        enterRoomATimeLine = EnterRoomATimeLineObj.GetComponent<PlayableDirector>();
        enterRoomCTimeLine = EnterRoomCTimeLineObj.GetComponent<PlayableDirector>();
        enterRoomEntranceRoomTimeLine = enterEntranceTimeLineObj.GetComponent<PlayableDirector>();
    }
    bool once = true;
    void Update() {
        if (searchPlayerFlag && !GameTrigger.gameOver) {
            //プレイヤー見つけたとき
            if (!PlayerStatus.isPlayerHide) {
                gameController.GameOver();
            }
        }

        AnimatorController();
    }

    //敵が部屋に入る関数
    public void OnStart(Room _room) {
        //プレイヤーを探すフラグ
        searchPlayerFlag = true;
        room = _room;

        /*
         処理の流れ
        ・ドアの開閉
        ・敵オブジェクトの場所と方向の初期化
        ・
         */

        switch (_room) {
            case Room.roomA:
                Debug.Log("roomA");
                onStartAnimate = true;
                enterRoomATimeLine.Play();
                //enterRoomATimeLineが終了したときに呼び出される
                enterRoomATimeLine.stopped += ExitRoom;

                doorController = doorA.GetComponent<DoorController>();
                doorController.EnterEnemyInRoom();
                
                break;

            case Room.roomC:
                Debug.Log("roomC");
                onStartAnimate = true;
                enterRoomCTimeLine.Play();

                enterRoomCTimeLine.stopped += ExitRoom;

                doorController = doorA.GetComponent<DoorController>();
                doorController.EnterEnemyInRoom();

                break;

            case Room.entrance:
                onStartAnimate = true;
                enterRoomEntranceRoomTimeLine.Play();
                enterRoomEntranceRoomTimeLine.stopped += GameOver;
                break;

            default:
                break;

        }
    }

    private void AnimatorController() {
        if (!onStartAnimate) return;

        animationTime += Time.deltaTime;

        if (isWalk(animationTime)) {
            enemyAnimator.SetBool("Walk", true);
        }
        else {
            enemyAnimator.SetBool("Walk", false);
        }

    }

    private bool isWalk(float _animationTime) {

        if (room == null) {
            Debug.Log("room is null");
            return false;
        }

        if (room == Room.roomA || room == Room.roomC) {
            return (0 <= _animationTime && _animationTime < 2) ||
            (2.5 <= _animationTime && _animationTime < 4.5) ||
            (5.5 <= _animationTime);
        }
        else {
            return (0 <= _animationTime && _animationTime <= 1) ||
                (1.5 <= _animationTime);
        }
    }

    //敵がドアを締める関数
    private void ExitRoom() {
        Debug.Log("hoge");
        doorController.ExitEnemyFromRoom();
        searchPlayerFlag = false;
    }

    //PlayableDirectorから呼ばれる関数
    private void ExitRoom(PlayableDirector director) {
        onStartAnimate = false;
        animationTime = 0f;
        doorController.ExitEnemyFromRoom();
        searchPlayerFlag = false;
        enemyController.RefreshPosition();
    }

    private void GameOver(PlayableDirector director) {
        gameController.GameOver();
    }
}
