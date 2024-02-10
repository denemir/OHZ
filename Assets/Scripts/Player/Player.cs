using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    //player stats & profile
    public int id { get; private set; }
    public Profile playerProfile;
    public string playerName = "player"; //player chooses, player_id by default
    public PlayerStats playerStats;

    //name tag
    private NameTag nameTag;
    public GameObject nameTagPrefab;

    //clan tag
    private int clanTagLength = 5;
    public char[] clanTag; //max 5 characters in a clantag
    public bool playerHasClanTag = false;

    //camera
    public GameObject cameraPrefab;
    public Camera activeCamera; //which camera player is currently watching

    //character & model (+ children)
    public Character currentCharacter;
    private GameObject characterObject;
    public GameObject characterPrefab;
    public RightHand rightHand;

    //arrow indicator
    private ArrowIndicator arrowIndicator;
    public GameObject arrowIndicatorPrefab;
    public GameObject arrowObject;

    //movement
    public float moveSpeed = 5f;
    private PlayerRotation playerRotation;
    public float rotationAngle;
    public bool isSprintToggle;
    public bool sprintToggled;

    //weapons & inventory
    private PlayerInventory playerInventory;
    private PlayerPerks playerPerks;

    //input
    private float horizontalInput;
    private float verticalInput;
    private bool controllerRightTriggerPress;

    //movement states
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting
    }
    public MovementState movementState = MovementState.Idle;

    //input state
    public enum InputState
    {
        KandM,
        Controller
    }
    public InputState inputState;

    //life state
    public enum DownState
    {
        Alive,
        Downed,
        Reviving,
        Dead
    }
    public DownState downState;

    //pausing 
    public PauseMenuHandler pauseMenuHandler;

    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.identity; //reset rotation
        InitializePlayer();

        //clan tag
        clanTag = new char[clanTagLength];
    }

    // Update is called once per frame
    void Update()
    {
        //input
        if (!pauseMenuHandler.currentlyPaused())
        {
            //firing and reloading inputs
            if (playerInventory != null && playerInventory.GetCurrentWeapon() != null)
            {
                CheckShootCurrentWeapon();
                CheckReloadCurrentWeapon();
                CheckReloadCancel();

                //weapon swapping
                switch(inputState)
                { 
                    case InputState.KandM:
                        CheckSwapToPrimaryWeapon();
                        CheckSwapToSecondaryWeapon();
                        break;
                    case InputState.Controller:
                        CheckCycleWeapons();
                        CheckSprintToggle();
                        break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (characterObject == null)
            characterObject = currentCharacter.model;
        //input
        if (!pauseMenuHandler.currentlyPaused())
        {
            //input
            getMovementInput();
            CheckJump();
            MoveCharacter(horizontalInput, verticalInput);


            //player rotation manager
            RotationHandler();
        }
    }

    //player initialization
    private void InitializePlayer()
    {
        InitializePlayerInventory();
        InitializePlayerPerks();
        InitializeCharacterModel();
        InitializeArrowIndicator();
        InitializeCamera();
        InitializeNameTag();
    }
    private void InitializePlayerInventory()
    {
        playerInventory = GetComponent<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory component not found on the player object. Please add the PlayerInventory component to the player object.");
        }
    }
    private void InitializePlayerPerks()
    {
        playerPerks = GetComponent<PlayerPerks>();
        if (playerPerks == null)
        {
            Debug.LogError("PlayerPerks component not found on the player object. Please add the PlayerPerks component to the player object.");
        }
    }
    private void InitializeCharacterModel()
    {
        //set rotation values
        playerRotation = GetComponent<PlayerRotation>();
        rotationAngle = playerRotation.rotationAngle; //determines mouse direction immediately

        if (GetComponentInChildren<Character>() != null)
        {
            currentCharacter = GetComponentInChildren<Character>();
            currentCharacter.transform.position = transform.position;
            characterObject = currentCharacter.model;
            CheckRightHand();
        }
        else Debug.Log("Character not found in prefab for player " + name + ".");
    }

    private void InitializeArrowIndicator()
    {
        //player arrow direction indicator
        if (arrowIndicatorPrefab != null)
        {
            arrowObject = Instantiate(arrowIndicatorPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            arrowIndicator = arrowObject.GetComponent<ArrowIndicator>();
            arrowIndicator.transform.SetParent(transform);
            arrowIndicator.playerTransform = transform;
        }
        else Debug.Log("Arrow indicator prefab not found on player " + name + ".");
    }
    private void InitializeNameTag()
    {
        if (nameTagPrefab != null)
        {
            GameObject newTagObject = Instantiate(nameTagPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newTagObject.transform.SetParent(transform);
            nameTag = newTagObject.GetComponent<NameTag>();
            nameTag.target = this;
        }
        else Debug.Log("No nametag prefab found.");
    }
    private void InitializeCamera()
    {
        //GameObject cameraInstance = Instantiate(cameraPrefab);
        //activeCamera = cameraInstance.GetComponent<Camera>();
        activeCamera.GetComponent<CameraFollow>().target = transform;
        activeCamera.GetComponent<AudioListener>().enabled = true;
        //cameraInstance.GetComponent<AudioListener>().enabled = true;
        activeCamera.GetComponent<VerticalVisibility>().SetTarget(this);

    }
    private void InitializeClanTag()
    {
        //do this later :D
    }

    //player input
    private void getMovementInput()
    {
        //implement switch case to swap between controller & keyboard

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        ModifyMovementState(horizontalInput, verticalInput);

        if(inputState == InputState.Controller)
        {
            isSprintToggle = true;
        }
    }
    private void ModifyMovementState(float horizontalInput, float verticalInput)
    {
        switch (horizontalInput != 0f || verticalInput != 0f)
        {
            case true:
                if (!isSprintToggle && Input.GetButton("Sprint")) //hold
                {

                    movementState = MovementState.Sprinting;

                }
                else if(isSprintToggle && sprintToggled)
                {
                    movementState = MovementState.Sprinting;
                }
                else movementState = MovementState.Walking;
                break;
            case false:
                movementState = MovementState.Idle;
                break;
        }
    }
    private void MoveCharacter(float horizontalInput, float verticalInput)
    {
        GetComponent<NewMovementHandler>().Move(horizontalInput, verticalInput, /*moveSpeed,*/ (int)movementState); //calling for move function in movement handler component
        characterObject.transform.position = transform.position;
    } //move character based on input

    //rotations
    private void RotationHandler()
    {
        rotationAngle = playerRotation.rotationAngle;
        arrowIndicator.Rotate(rotationAngle);
        currentCharacter.RotateRightHand(rotationAngle);
    } //handles rotating the player (and children) to face either towards mouse or right thumbstick direction


    //character model methods
    public bool DoesCharacterHaveRightHand()
    {
        if (currentCharacter == null)
            Debug.Log("sacks");
        if (currentCharacter.GetComponentInChildren<MeshRenderer>()?.gameObject.GetComponentInChildren<RightHand>() != null)
            return true;
        Debug.Log("Character right hand does not exist.");
        return false;
    } //determines if character model has rightHand object
    public RightHand GetRightHand()
    {
        return currentCharacter.rightHand;
    }
    public void CheckRightHand()
    {
        if (DoesCharacterHaveRightHand())
        {
            currentCharacter.rightHand = GetComponentInChildren<MeshRenderer>()?.gameObject.GetComponentInChildren<RightHand>();
        }
    }


    //weapon & inventory inputs
    public Weapon GetCurrentWeapon()
    {
        if (playerInventory != null)
        {
            return playerInventory.GetCurrentWeapon();
        }
        return null;
    }
    private void CheckShootCurrentWeapon()
    {
        Weapon weapon = GetComponent<PlayerInventory>().GetCurrentWeapon();
        switch (weapon.isFullAuto)
        {
            case true: //weapon is automatic
                switch(inputState)
                {
                    case InputState.KandM:
                        if (Input.GetButton("Fire1"))
                        {
                            weapon.Shoot(transform);                        //fix shotgun spread 
                        }
                        break;
                    case InputState.Controller:
                        if (Input.GetAxis("Fire1") == 1)
                        {
                            weapon.Shoot(transform);
                        }
                        break;
                }
                break;
            case false: //weapon is semi auto

                switch (inputState)
                {
                    case InputState.KandM:
                        if (Input.GetButtonDown("Fire1"))
                        {
                            weapon.Shoot(transform);                        //fix shotgun spread 
                        }
                        break;
                    case InputState.Controller:
                        if (Input.GetAxis("Fire1") == 1 && !controllerRightTriggerPress)
                        {
                            weapon.Shoot(transform);
                            controllerRightTriggerPress = true;
                        }
                        else if (Input.GetAxis("Fire1") == 0)
                            controllerRightTriggerPress = false;
                        break;
                }
                break;
        }
    }
    private void CheckReloadCurrentWeapon()
    {
        Weapon weapon = GetComponent<PlayerInventory>().GetCurrentWeapon();
        //reload
        if (Input.GetButtonDown("Reload") && weapon.currentAmmoInMag < weapon.magazineSize && weapon.currentStockAmmo > 0)
        {
            weapon.BeginReloading();
            if(sprintToggled)
                sprintToggled = false;
            //Debug.Log("Reloading...");
        }
    }
    private void CheckReloadCancel()
    {
        //reload interrupt/cancel
        if (playerInventory.GetCurrentWeapon().reloadState == Weapon.ReloadState.Reloading && Input.GetButton("Sprint") || sprintToggled)
        {
            playerInventory.GetCurrentWeapon().CancelReload();
        }
    } //if player presses Sprint key, cancel reload
    private void CheckSwapToPrimaryWeapon()
    {
        if (!Input.GetButton("Fire1")/* && playerInventory.isSwapWeaponTimerZero()*/ && playerInventory.GetCurrentWeaponSlot() != 0 && Input.GetButtonDown("Primary Weapon"))
        {
            playerInventory.SwapCurrentWeapon(0);
            //Debug.Log("0");
        }
    }
    private void CheckSwapToSecondaryWeapon()
    {
        if (!Input.GetButton("Fire1") /*&& playerInventory.isSwapWeaponTimerZero()*/ && playerInventory.GetCurrentWeaponSlot() != 1 && Input.GetButtonDown("Secondary Weapon"))
        {
            playerInventory.SwapCurrentWeapon(1);
        }
    }
    private void CheckCycleWeapons() //primarily for controller players, but can also be set for K&M players
    {
        if (!Input.GetButton("Fire1") && Input.GetButtonDown("Cycle Weapons (Controller)"))
            playerInventory.CycleCurrentWeapon();
    }
    private void CheckSprintToggle()
    {
        switch(inputState)
        {
            case InputState.Controller:
                if(Input.GetButtonDown("Sprint"))
                {
                    sprintToggled = !sprintToggled;
                }                    
                break;
        }
    }
    private void CheckJump()
    {
        switch (inputState) //////////////////fix later
        {
            case InputState.KandM:
                if (Input.GetAxisRaw("Jump") != 0)
                {
                    GetComponent<NewMovementHandler>().Jump();
                }
                break;
            case InputState.Controller:
                if (Input.GetAxisRaw("Jump") != 0)
                {
                    GetComponent<NewMovementHandler>().Jump();
                }
                break;
        }

    }

    private bool DoesPlayerInventoryComponentExist()
    {
        return GetComponent<PlayerInventory>() != null;
    }

    //getters
    public PlayerInventory GetPlayerInventory()
    {
        return playerInventory;
    }
    public PlayerPerks GetPlayerPerks()
    {
        return playerPerks;
    }
}
