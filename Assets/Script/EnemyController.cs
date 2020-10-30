using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{


    [SerializeField] private Transform player;

    [HideInInspector] public Animator enemyAnimator;

    private Vector3 target;

    private Vector3 defaultEnemyPos;

    private bool animatorFirstTrigger;
    private bool animatorSecondTrigger;
    private bool exit;

    private Transform startPos; //
    private Transform idlePos;
    private Action exitAction;
    private bool isOnce = true;

    private float stopTime = 4.0f;


    void Start() {
        enemyAnimator = GetComponent<Animator>();
        defaultEnemyPos = gameObject.transform.position;
    }

    void Update() {

        if (animatorFirstTrigger) {
            //静止処理
            enemyAnimator.SetBool("Walk", false);
            //agent.enabled = false;
            animatorFirstTrigger = false;

            //stopTime後動き出す
            StartCoroutine(DelayMethod(stopTime, () =>
            {
                animatorSecondTrigger = true;
                enemyAnimator.SetBool("Walk", true);
            }));
        }

        //
        if (animatorSecondTrigger && enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
            SetAgent(startPos);

            if (exit && isOnce) {
                isOnce = false;
                exitAction();
            }
        }
    }

    //NavMeshの方向を決める関数
    public void SetAgent(Transform targetTrans) {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        //if (!agent.enabled) agent.enabled = true;

        //agent.SetDestination(targetTrans.position);
    }

    //もとの位置に戻す関数
    public void RefreshPosition() {
        gameObject.transform.position =  defaultEnemyPos;
    }

    //called by EnemyEnterRoomProcess.cs
    //
    public void EnterRoom(Transform startPos, Transform idlePos, Action exitAction) {

        this.startPos = startPos;
        this.idlePos  = idlePos;
        this.exitAction = exitAction;

        SetAgent(idlePos);
        enemyAnimator.SetBool("Walk", true);
    }

    //停止地点に来たときに呼び出される関数
    public void IdleTrigger(Collider other) {
        if (other.tag == "Enemy") {
            Debug.Log("trigger");
            exit = false;
            animatorFirstTrigger = true; //Update関数に続く
        }
    }

    //
    public void ExitTrigger(Collider other) {
        if (other.tag == "Enemy") {
            exit = true;
        }
    }

    IEnumerator DelayMethod(float delaytime, Action action) {
        yield return new WaitForSeconds(delaytime);
        action();
    }

    IEnumerator waitAnimation(bool b, Action action) {
        yield return new WaitUntil(() => b);

        action();
    }
}
