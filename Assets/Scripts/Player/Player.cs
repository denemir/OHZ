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

    //name tag
    private NameTag nameTag;
    public GameObject nameTagPrefab;

    //leaderboard stats
    public int points;
    public int kills;
    public int downs;
    public int revives;

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

    //weapons & inventory
    //public GameObject currentWeaponPrefab; //for spawn weapon
    ////public GameObject secondaryWeaponPrefab;
    ////public GameObject tertiaryWeaponPrefab;
    private PlayerInventory playerInventory;


    //public Weapon currentWeapon;
    //public int currentWeaponSlot { get; private set; }

    //public Weapon primaryWeapon { get; private set; }
    //public Weapon secondaryWeapon { get; private set; }
    //public Weapon tertiaryWeapon { get; private set; }
    //public bool isTertiaryWeaponActive = false; //false by default

    //input
    private float horizontalInput;
    private float verticalInput;

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
        if (characterObject == null)
            characterObject = currentCharacter.model; //test

        if (playerInventory != null && playerInventory.GetCurrentWeapon() != null)
        {
            CheckShootCurrentWeapon();
            CheckReloadCurrentWeapon();
            CheckReloadCancel();
        }
        //input
        getInput();
        MoveCharacter(horizontalInput, verticalInput);

        //player rotation manager
        RotationHandler();

        ////check if hand dropped weapon
        //if (currentCharacter.rightHand.droppedWeapon)
        //{
        //    DropWeapon();
        //}

    }

    //player initialization
    private void InitializePlayer()
    {
        InitializePlayerInventory();
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
    private void InitializeCharacterModel()
    {
        //set rotation values
        playerRotation = GetComponent<PlayerRotation>();
        rotationAngle = playerRotation.rotationAngle; //determines mouse direction immediately

        if (GetComponentInChildren<Character>() != null)
        {
            currentCharacter = GetComponentInChildren<Character>();
            currentCharacter.transform.position = transform.position;
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
        GameObject cameraInstance = Instantiate(cameraPrefab);
        activeCamera = cameraInstance.GetComponent<Camera>();
        activeCamera.GetComponent<CameraFollow>().target = transform;
        activeCamera.GetComponent<AudioListener>().enabled = false;

    }
    private void InitializeClanTag()
    {
        //do this later :D
    }
    private void InitializeWeapon(Weapon newWeaponPrefab)
    {

    } //differs from primary weapon because this can be called by the SwapWeaponMethod

    //player input
    private void getInput()
    {
        //implement switch case to swap between controller & keyboard

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        ModifyMovementState(horizontalInput, verticalInput);

        if (GetCurrentWeapon() != null)
        {
            //firing
            CheckShootCurrentWeapon();

            CheckReloadCurrentWeapon();

            //reload interrupt/cancel
            CheckReloadCancel();
        }

        //weapon swapping
        CheckSwapToPrimaryWeapon();
        CheckSwapToSecondaryWeapon();
    }
    private void ModifyMovementState(float horizontalInput, float verticalInput)
    {
        switch (horizontalInput != 0f || verticalInput != 0f)
        {
            case true:
                if (Input.GetButton("Sprint")) //hold
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

    //weapon methods
    public Weapon GetCurrentWeapon()
    {
        if (playerInventory != null)
        {
            return playerInventory.GetCurrentWeapon();
        }
        return null;
    }

    //character model methods
    public bool doesCharacterHaveRightHand()
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
        if (doesCharacterHaveRightHand())
        {
            currentCharacter.rightHand = GetComponentInChildren<MeshRenderer>()?.gameObject.GetComponentInChildren<RightHand>();
        }
        characterObject = currentCharacter.model;
    }
    //private bool isCharacterHoldingWeapon()
    //{
    //    if (!currentCharacter.rightHand.holdingWeapon)
    //        return false; //character is not holding a weapon
    //    //Debug.Log("Character is already holding a weapon.");
    //    return true;
    //}

    //checks (input checks)                      //////////////////////////////////////////////////////////////////////////////////////set to use methods in PlayerInventory component
    private void CheckShootCurrentWeapon()
    {
        Weapon weapon = GetComponent<PlayerInventory>().GetCurrentWeapon();
        switch (weapon.isFullAuto)
        {
            case true:

                if (Input.GetButton("Fire1"))
                {
                    weapon.Shoot(transform);                        //fix shotgun spread 
                }
                break;

            case false:

                if (Input.GetButtonDown("Fire1"))
                {
                    weapon.Shoot(transform);
                    //Debug.Log("Shot");
                    //Debug.Log(weapon.currentAmmoInMag);
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
            //Debug.Log("Reloading...");
        }
    }
    private void CheckReloadCancel()
    {
        //reload interrupt/cancel
        if (playerInventory.GetCurrentWeapon().reloadState == Weapon.ReloadState.Reloading && Input.GetButton("Sprint"))
        {
            playerInventory.GetCurrentWeapon().CancelReload();
        }
    }
    private void CheckSwapToPrimaryWeapon()
    {
        if(!Input.GetButton("Fire1") && playerInventory.isSwapWeaponTimerZero() && playerInventory.GetCurrentWeaponSlot() != 0 && Input.GetButtonDown("Primary Weapon"))
        {
            playerInventory.SwapCurrentWeapon(0);
        }
    }
    private void CheckSwapToSecondaryWeapon()
    {
        if (!Input.GetButton("Fire1") && playerInventory.isSwapWeaponTimerZero() && playerInventory.GetCurrentWeaponSlot() != 1 && Input.GetButtonDown("Secondary Weapon"))
        {
            playerInventory.SwapCurrentWeapon(1);
        }
    }

    private bool DoesPlayerInventoryComponentExist()
    {
        return GetComponent<PlayerInventory>() != null;
    }

    //debug methods (remove later)
    private void hasWeaponChanged()
    {

    } //for manually initiated weapon swap in unity inspector

    //getters
    public PlayerInventory GetPlayerInventory()
    {
        return playerInventory;
    }
}
