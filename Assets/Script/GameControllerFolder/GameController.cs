﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using AudioManager;

public class GameController : MonoBehaviour {

    public GameObject player;

    public GameObject enemy;

    [Space(10)]

    [Header("UI")]
    [SerializeField] private GameObject messageUI;
    [HideInInspector] public MessageController messageController;

    [SerializeField] private GameObject actionNavigator;
    [HideInInspector] public ActionNavigationController actionNavigationController;

    [SerializeField] private GameObject padLockUI;
    [HideInInspector] public PadLockUIController padLockUIController;

    [SerializeField] private GameObject choiceUI;
    [HideInInspector] public ChoiceUIController choiceUIController;

    [SerializeField] private GameObject passwordUI;
    [HideInInspector] public PasswordController passwordController;

    [SerializeField] private GameObject riddleUI;
    [HideInInspector] public RiddleUIController riddleUIController;

    [SerializeField] private GameObject countDownUI;
    [HideInInspector] public CountDownUIController countDownUIController;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject currentHasItemTextObj;
     private Text currentHasItemText;

    [SerializeField] private GameObject eightPuzzleObj;
    [HideInInspector] public EightPuzzle eightPuzzle;
    public GameObject eightPuzzleUI;

    [SerializeField] private GameObject editUI;
    [HideInInspector] public Edit edit;

    [HideInInspector] public EnemyEnterRoomProcess enemyEnterProcess;

    [Space(10)]

    [Header("monitor")]
    [SerializeField] private GameObject roomCMonitorObj;
    [HideInInspector] public AirConMonitor roomCMonitor;

    [SerializeField] private GameObject entranceRoomMonitorObj;
    [HideInInspector] public AirConMonitor entranceRoomMonitor;

    [SerializeField] private GameObject bookShelf;
    private BookShelfController bookShelfController;

    [Space(10)]

    [Header("CutScene")]
    [SerializeField] private GameObject openingTimeLine;
    private PlayableDirector openingDirector;

    [Space(10)]

    [Header("Camera")]
    public GameObject mainCamera;
    public GameObject padLockCamera;
    [SerializeField] private GameObject openingCutSceneCamera;

    public bool ActionNavigatorActiveSelf {
        get { return actionNavigator.activeSelf; }
    }

    private void Awake() {
        messageController = messageUI.GetComponent<MessageController>();
        actionNavigationController = actionNavigator.GetComponent<ActionNavigationController>();
        padLockUIController = padLockUI.GetComponent<PadLockUIController>();
        choiceUIController = choiceUI.GetComponent<ChoiceUIController>();
        passwordController = passwordUI.GetComponent<PasswordController>();
        riddleUIController = riddleUI.GetComponent<RiddleUIController>();
        countDownUIController = countDownUI.GetComponent<CountDownUIController>();
        eightPuzzle = eightPuzzleObj.GetComponent<EightPuzzle>();
        enemyEnterProcess = GetComponent<EnemyEnterRoomProcess>();
        edit = GetComponent<Edit>();

        currentHasItemText = currentHasItemTextObj.GetComponent<Text>();

        if (eightPuzzleUI.activeSelf) eightPuzzleUI.SetActive(false);
        if (editUI) editUI.SetActive(false);

        roomCMonitor = roomCMonitorObj.GetComponent<AirConMonitor>();
        entranceRoomMonitor = entranceRoomMonitorObj.GetComponent<AirConMonitor>();

        bookShelfController = bookShelf.GetComponent<BookShelfController>();

        padLockCamera.SetActive(false);
        playerUI.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        BGMManager.Instance.Play(
            audioPath:BGMPath.MAIN_BGM,
            volumeRate: GameSettings.getBGMRatio
        );

        openingDirector = openingTimeLine.GetComponent<PlayableDirector>();
        player.SetActive(false);

        StartCoroutine(DelaySecond(8.0f, () =>
        {
            player.SetActive(true);
            playerUI.SetActive(true);
            openingCutSceneCamera.SetActive(false);
        }));
    }


    void Update() {

        if (IsPlayerUIActive && Input.GetKeyDown(KeyCode.Q)) {
            PushSettingScene();
        }

        if (GameTrigger.isPlayerHasDriverTip && !GameTrigger.isShakeFloor) {

            if (!messageController.isClose) return;

            GameTrigger.isShakeFloor = true;
            GameTrigger.isEvent = true;

            //地震のサウンド。カット予定
            //audioSource.PlayOneShot(earthquake);

            StartCoroutine(DelaySecond(1.0f, () =>
            {
                //audioSource.Stop();
                GameTrigger.isEvent = false;
                EnemywalkSoundPlay = true;

                StartCoroutine(DelaySecond(1.0f, () =>
                {
                    messageController.SetMessagePanel(MessageText.somethingApproach());

                    StartCoroutine(waitCloseMessageUI(() => {
                        countDownUIController.CountDownStart(initialValue: 5.0f, Room.roomA);

                    }));
                }));
            }));


        }
    }

    public void PushSettingScene() {
        ClearUI();

        GameTrigger.isEventScene = true;

        editUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }

    public void PopSettingScene() {
        Pop(editUI);
    }

    public void ClearUI() {
        actionNavigationController.Active(false);
        playerUI.SetActive(false);

        messageController.Active(false);
        choiceUI.SetActive(false);
    }

    public void Pop(GameObject ui) {
        GameTrigger.isEventScene = false;
        ui.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerUI.SetActive(true);
    }

    //playerUIのSetActiveを外部で切り替える関数
    public void ChangePlayerUIActive(bool b) {
        playerUI.SetActive(b);
    }

    public bool IsPlayerUIActive {
        get { return playerUI.activeSelf; }
    }

    public string SetCurrentHasItem {
        set { currentHasItemText.text = value; }
    }

    public void GameOver() {
        GameTrigger.gameOver = true;
        GameTrigger.isEventScene = true;
        playerUI.SetActive(false);

        BGMManager.Instance.Stop();

        //Idea:敵がプレイヤーを殴るシーン

        messageController.SetMessagePanel(MessageText.GameOver());

        StartCoroutine(waitCloseMessageUI( () => { SceneManager.LoadScene("GameOverScene"); }));
    }

    public void GameClear() {
        GameTrigger.gameOver = false;
        GameTrigger.isEventScene = true;

        BGMManager.Instance.Stop();

        SceneManager.LoadScene("GameOverScene");
    }

    public IEnumerator waitCloseMessageUI(Action action) {
        yield return new WaitUntil(() => messageController.isClose);

        action();
    }

    public IEnumerator DelaySecond(float delaytime, Action action) {
        yield return new WaitForSeconds(delaytime);
        action();
    }

    //Enemy Enter room process


    [Header("Sound")]
    [SerializeField] private AudioClip earthquake;
    private AudioSource audioSource;

    public AudioClip wideDrawerSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public AudioClip unLockSource;
    public AudioClip rotateDialSource;
    public AudioClip showSpriteSound;

    private bool EnemywalkSoundPlay;

    public void AudioPlay (AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void AudioPlayOneShot(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

}
