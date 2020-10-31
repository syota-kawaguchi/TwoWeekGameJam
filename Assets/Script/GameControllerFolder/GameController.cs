using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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

    [HideInInspector] public EnemyEnterRoomProcess enemyEnterProcess;

    [Space(10)]

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

    [Header("temperature")]
    [SerializeField]private float defaultRoomCTemperature = 27.0f;
    public float roomCTemperature;

    [SerializeField] private float defaultEntranceRoomTemperature = 27.0f;
    public float entranceRoomtemperature;

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
        currentHasItemText = currentHasItemTextObj.GetComponent<Text>();

        bookShelfController = bookShelf.GetComponent<BookShelfController>();

        padLockCamera.SetActive(false);
        playerUI.SetActive(false);

        roomCTemperature = defaultRoomCTemperature;
        entranceRoomtemperature = defaultEntranceRoomTemperature;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
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

    public void ClearUI() {
        actionNavigationController.Active(false);
        playerUI.SetActive(false);

        messageController.Active(false);
        choiceUI.SetActive(false);
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

    public void UpRoomTemperature(bool isRoomC) {
        if (isRoomC) {
            if (roomCTemperature >= 30) return;

            roomCTemperature += 1;
        }
        else {
            if (entranceRoomtemperature >= 30) return;

            entranceRoomtemperature += 1;
        }
    }

    public void DownRoomTemperature(bool isRoomC) {
        if (isRoomC) {
            if (roomCTemperature <= 10) return;

            roomCTemperature -= 1;
        }
        else {
            if (entranceRoomtemperature <= 10) return;

            entranceRoomtemperature -= 1;
        }
    }

    public void GameOver() {

        GameTrigger.gameOver = true;
        GameTrigger.isEventScene = true;
        playerUI.SetActive(false);

        //Idea:敵がプレイヤーを殴るシーン

        messageController.SetMessagePanel(MessageText.GameOver());

        StartCoroutine(waitCloseMessageUI( () => { SceneManager.LoadScene("GameOverScene"); }));
    }

    public void GameClear() {
        GameTrigger.gameOver = false;
        GameTrigger.isEventScene = true;

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
    public AudioClip doorSource;
    public AudioClip unLockSource;

    private bool EnemywalkSoundPlay;

    public void AudioPlay (AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void AudioPlayOneShot(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    } 
}
