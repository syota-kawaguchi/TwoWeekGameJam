using UnityEngine;
using System.Collections;


public class MouseLookScript : MonoBehaviour {

	[HideInInspector]
	public Transform myCamera;

    private PlayerMovementScript playerMovementScript;

    [SerializeField] private GameObject gameControllerObj;
    private GameController gameController;
    private Edit edit;


	void Awake(){
        HideCursor();
	}
    private void Start() {
        playerMovementScript = GetComponent<PlayerMovementScript>();
        gameController = gameControllerObj.GetComponent<GameController>();
        edit = gameControllerObj.GetComponent<Edit>();
    }

    void  Update(){

		MouseInputMovement();

		if (Input.GetKeyDown (KeyCode.L)) {
			if (!GameTrigger.playerDisableMove)Cursor.lockState = CursorLockMode.Locked;
		}

        if (playerMovementScript.currentSpeed > 1 && !GameTrigger.playerDisableMove) {
            Cursor.lockState = CursorLockMode.Locked;
            myCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
	}

    private void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        myCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    [Header("Z Rotation Camera")]
	[HideInInspector] public float timer;
	[HideInInspector] public int int_timer;
	[HideInInspector] public float zRotation;
	[HideInInspector] public float wantedZ;
	[HideInInspector] public float timeSpeed = 2;

	[HideInInspector] public float timerToRotateZ;

	void HeadMovement(){
		timer += timeSpeed * Time.deltaTime;
		int_timer = Mathf.RoundToInt (timer);
		if (int_timer % 2 == 0) {
			wantedZ = -1;
		} else {
			wantedZ = 1;
		}

		zRotation = Mathf.Lerp (zRotation, wantedZ, Time.deltaTime * timerToRotateZ);
	}
	[Tooltip("Current mouse sensivity, changes in the weapon properties")]
	public float mouseSensitvity = 0;
	[HideInInspector]
	public float mouseSensitvity_notAiming = 300;
	[HideInInspector]
	public float mouseSensitvity_aiming = 50;


    void FixedUpdate(){

	    if(Input.GetAxis("Fire2") != 0){
		    mouseSensitvity = mouseSensitvity_aiming * edit.MouseSensitivilityRatio;
	    }
	    else if(playerMovementScript.maxSpeed > 5){
		    mouseSensitvity = mouseSensitvity_notAiming * edit.MouseSensitivilityRatio;
	    }
	    else{
		    mouseSensitvity = mouseSensitvity_notAiming * edit.MouseSensitivilityRatio;
	    }


	    ApplyingStuff();


    }


    private float rotationYVelocity, cameraXVelocity;
    [Tooltip("Speed that determines how much camera rotation will lag behind mouse movement.")]
    public float yRotationSpeed, xCameraSpeed;

    [HideInInspector]
    public float wantedYRotation;
    [HideInInspector]
    public float currentYRotation;

    [HideInInspector]
    public float wantedCameraXRotation;
    [HideInInspector]
    public float currentCameraXRotation;

    [Tooltip("Top camera angle.")]
    public float topAngleView = 60;
    [Tooltip("Minimum camera angle.")]
    public float bottomAngleView = -45;
    /*
     * Upon mouse movenet it increases/decreased wanted value. (not actually moving yet)
     * Clamping the camera rotation X to top and bottom angles.
     */
    void MouseInputMovement(){

        if (GameTrigger.playerDisableMove) return;

	    wantedYRotation += Input.GetAxis("Mouse X") * mouseSensitvity;

	    wantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitvity;

	    wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);

    }

    /*
     * Smoothing the wanted movement.
     * Calling the waeponRotation form here, we are rotating the waepon from this script.
     * Applying the camera wanted rotation to its transform.
     */
    void ApplyingStuff(){

	    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
	    currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

	    transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
	    myCamera.localRotation = Quaternion.Euler(currentCameraXRotation, 0, zRotation);

    }
}
