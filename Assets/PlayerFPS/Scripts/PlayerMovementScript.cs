using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementScript : MonoBehaviour {

	private Rigidbody rb;
    private Camera mainCamera;

    public int maxSpeed = 5;
    public float currentSpeed;
    public float deaccelerationSpeed = 15.0f;
    public float accelerationSpeed   = 50000.0f;
    public float jumpForce           = 500;

    private Vector3 slowdownV;
    private Vector2 horizontalMovement;

    public bool grounded;

    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;

    [HideInInspector] public Transform mainCameraTrans;
    [HideInInspector] public Vector3 cameraPosition;

    private Ray utilizeRay;
    [SerializeField] private float rayDistancee = 4.0f;

    private int UIActionLayerMask;

    [Header("Shooting Properties")]
	private LayerMask ignoreLayer;//to ignore player layer

    private GameObject utilizeObject; // プレイヤーが作用させることができるもの

    private KeyCode keyCode = KeyCode.None;
    private string messageText;
    private string actionText;
    private Action action;

    private ItemController itemController;
    private FurnitureScript furnitureController;

	void Awake(){
		rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
		mainCameraTrans = mainCamera.transform;

        ignoreLayer = 1 << LayerMask.NameToLayer ("Player");
        UIActionLayerMask = LayerMask.GetMask(new string[] { "Furniture", "Item", "Wall"});
    }

    void Start() {
        gameController = gameControllerObj.GetComponent<GameController>();
    }

    void Update() {
        SetUtilizeObject();

        ShowUINavigation();

        ActionUtilize();

        WalkingSound();
    }

    void FixedUpdate(){
		PlayerMovementLogic ();
	}

	void PlayerMovementLogic(){
        if (GameTrigger.playerDisableMove || PlayerStatus.isPlayerHide) return;

		currentSpeed = rb.velocity.magnitude;
		horizontalMovement = new Vector2 (rb.velocity.x, rb.velocity.z);
		if (horizontalMovement.magnitude > maxSpeed){
			horizontalMovement = horizontalMovement.normalized;
			horizontalMovement *= maxSpeed;    
		}

		rb.velocity = new Vector3 (
			horizontalMovement.x,
			rb.velocity.y,
			horizontalMovement.y
		);

		if (grounded){
			rb.velocity = Vector3.SmoothDamp(rb.velocity,
				new Vector3(0,rb.velocity.y,0),
				ref slowdownV,
				deaccelerationSpeed
            );
		}

		if (grounded) {
			rb.AddRelativeForce (
                Input.GetAxis ("Horizontal") * accelerationSpeed * Time.deltaTime,
                0,
                Input.GetAxis ("Vertical") * accelerationSpeed * Time.deltaTime
            );
		} 
        else {
			rb.AddRelativeForce (
                Input.GetAxis ("Horizontal") * accelerationSpeed / 2 * Time.deltaTime,
                0,
                Input.GetAxis ("Vertical") * accelerationSpeed / 2 * Time.deltaTime
            );
		}

		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			deaccelerationSpeed = 0.5f;
		} 
        else {
			deaccelerationSpeed = 0.1f;
		}
	}

    private bool RayCastGrounded(){
		RaycastHit groundedInfo;
		if(Physics.Raycast(transform.position, transform.up *-1f, out groundedInfo, 1, ~ignoreLayer)){
			Debug.DrawRay (transform.position, transform.up * -1f, Color.red, 0.0f);
			if(groundedInfo.transform != null){
				return true;
			}
			else{
				return false;
			}
		}
		return false;
	}

    void OnCollisionStay(Collision other){
		foreach(ContactPoint contact in other.contacts){
			if(Vector2.Angle(contact.normal,Vector3.up) < 60){
				grounded = true;
			}
		}
	}

    void OnCollisionExit() {
        grounded = false;
    }

    //以下ActionLogic

    //作用させるオブジェクトの獲得
    void SetUtilizeObject() {
        if (!mainCamera) {
            Debug.Log("mainCamera is null");
            return;
        }

        var rayOrigine = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        utilizeRay = new Ray(rayOrigine, mainCameraTrans.forward * rayDistancee);

        Debug.DrawRay(rayOrigine, utilizeRay.direction * rayDistancee, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(utilizeRay, out hit, rayDistancee, UIActionLayerMask)) {
            utilizeObject = hit.collider.gameObject;
            Debug.Log(hit.collider.name);
        }
        else {
            utilizeObject = null;
        }
    }

    void ShowUINavigation() {
        if (!utilizeObject) {

            itemController = null;
            furnitureController = null;

            ClearMessage();

            if (gameController.ActionNavigatorActiveSelf) {
                gameController.actionNavigationController.ClearActionNavigation();
            }
            return; 
        }

        if (GameTrigger.isEventScene) return;

        if (utilizeObject.CompareTag("Wall")) {
            print("wall -----");
            return;
        }

        if (utilizeObject.CompareTag("Item")) {
            Debug.Log("utilize Item : " + utilizeObject.name);
            if (itemController == null) itemController = utilizeObject.GetComponentInParent<ItemController>();
            itemController.HandItemUIInfo(ref actionText, ref keyCode, ref action);
        }
        else if (utilizeObject.CompareTag("Furniture")) {
            Debug.Log("utilize Furniture ; " + utilizeObject.name);
            if (furnitureController == null) furnitureController = utilizeObject.GetComponentInParent<FurnitureScript>();
            furnitureController.handFurnitureUIInfo(ref messageText, ref actionText, ref keyCode, ref action);
        }

        if (actionText != null) {
            Debug.Log("setActionNavi");
            gameController.actionNavigationController.SetActionNavigation(keyCode, actionText);
        }
    }

    void ActionUtilize() {
        if (actionText == null) return;

        if (GameTrigger.isEventScene) return;

        if (Input.GetKeyDown(keyCode)) {
            if (action != null) action();
        }
    }

    //ここまでaction logic

    private void ClearMessage() {
        keyCode = KeyCode.None;
        messageText = null;
        actionText = null;
        action = null;
    }

    [Header("Player SOUNDS")]
	[Tooltip("Jump sound when player jumps.")]
	public AudioSource _jumpSound;
	[Tooltip("Walk sound player makes.")]
	public AudioSource _walkSound;
	[Tooltip("Run Sound player makes.")]
	public AudioSource _runSound;

    void WalkingSound() {
        if (_walkSound && _runSound) {
            if (RayCastGrounded()) { //for walk sounsd using this because suraface is not straigh			
                if (currentSpeed > 1) {
                    //				print ("unutra sam");
                    if (maxSpeed == 3) {
                        //	print ("tu sem");
                        if (!_walkSound.isPlaying) {
                            //	print ("playam hod");
                            _walkSound.Play();
                            _runSound.Stop();
                        }
                    }
                    else if (maxSpeed == 5) {
                        //	print ("NE tu sem");

                        if (!_runSound.isPlaying) {
                            _walkSound.Stop();
                            _runSound.Play();
                        }
                    }
                }
                else {
                    _walkSound.Stop();
                    _runSound.Stop();
                }
            }
            else {
                _walkSound.Stop();
                _runSound.Stop();
            }
        }
        else {
            print("Missing walk and running sounds.");
        }

    }

    void Jumping() {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddRelativeForce(Vector3.up * jumpForce);
            if (_jumpSound)
                _jumpSound.Play();
            else
                print("Missig jump sound.");
            _walkSound.Stop();
            _runSound.Stop();
        }
    }

    //しゃがむ
    void Crouching() {
        if (Input.GetKey(KeyCode.C)) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 0.6f, 1), Time.deltaTime * 15);
        }
        else {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 15);
        }
    }
}

