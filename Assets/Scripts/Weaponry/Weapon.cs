using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    //info
    public string weaponName;
    public Sprite icon;

    //fire rate
    public float fireRate;
    private float fireRateTimer; //if != 0, can't shoot :P
    public bool isFullAuto;
    public bool canToggleFullAuto; //decides if weapon can fire full automatic

    //damage
    public float damage;
    public float criticalDamage;
    public float criticalChance;

    //reload && ammunition
    public enum ReloadState
    {
        Not_Reloading,
        Reloading
    }
    public ReloadState reloadState;
    public bool isActive; //if isn't active, do not add to timer (rework timer to be incr. based rather than clock based
    public float timeToReload;
    private float reloadTimer; //tracks duration of reload
    private float currentTimer; //current state of timer

    public int magazineSize;
    public int currentAmmoInMag;
    public int currentStockAmmo; //ammo in stockpile by default (starting ammo)
    public int maxStockAmmo;

    public bool isWeaponHandLoaded; //does the weapon have a magazine or is it hand-loaded?
    public int roundsReloadedPerInstance; //in case weapon is hand loaded, how many rounds are loaded each time?

    //recoil
    public float recoilSpread; //max spread (positive goes one way from the guns barrel, negative goes the other way)
    public float currentRecoilSpread; /*{ get; private set; }*/ //current spread
    public float recoilRate; //how much each individual shot spreads the recoil

    //handling
    public float weaponSwapTime;
    //how much time is added to the weapon swap. weapon swap time works like this: when swapping
    //between 2 weapons, the total time to swap accounts for both weapons. smaller weapons (such
    //as smgs and pistols, have much faster swap times. the swap time accounts for both pulling
    //out, and putting away. (ex., 1911 has swap value of 50. MAG-10 has swap value of 120. The
    //total time to swap would be 170.)

    //weapon typing
    public enum WeaponType
    {
        Pistol,
        SubmachineGun,
        AssaultRifle,
        Shotgun,
        BattleRifle,
        LMG,
        SniperRifle,
        Melee
    }

    public WeaponType weaponType;

    //model
    public GameObject weaponModelPrefab;
    public GameObject weaponModel;

    public Transform barrelTipTransform;
    public BarrelTip barrelTip;

    //bullet
    public GameObject bulletModelPrefab;
    public float bulletVelocity;
    public BulletPool bulletPool;

    //player & player gui
    public Player player;
    public Character character;
    public RightHand rightHand;
    public PlayerGUIHandler playerGUIHandler;

    //interactions
    public int cost; //if weapon is a wall buy, otherwise keep at 950

    // Start is called before the first frame update
    void Start()
    {
        roundsReloadedPerInstance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //backups
        if (weaponModel == null)
        {
            SetWeaponModel();
        }
        if(bulletPool == null)
            bulletPool = GetComponent<BulletPool>();

        if (barrelTip == null)
        {
            SetBarrelTip();
        }

        if (currentTimer <= 0 && reloadState == ReloadState.Reloading)
        {
            Reload();

            //in case of hand load
            if (isWeaponHandLoaded && currentAmmoInMag < magazineSize)
            {
                BeginReloading();
            }
            else reloadState = ReloadState.Not_Reloading;

        } //reload

    }

    void FixedUpdate()
    {
        //reloading weapon timer
        if (currentTimer > 0)
            currentTimer -= 0.005f;

        currentRecoilSpread = Mathf.Lerp(currentRecoilSpread, 0, Time.deltaTime * 1.5f); //decrease spread over time
    }

    //weapon actions

    //shooting
    public void Shoot(Transform shooterTransform)
    {
        switch (reloadState)
        {
            case ReloadState.Not_Reloading:
                if (Time.time > fireRateTimer && currentAmmoInMag > 0)
                {
                    switch (this is Shotgun)
                    {
                        case true:
                            GetComponent<Shotgun>().Fire(shooterTransform);
                            break;
                        default:
                            Fire(shooterTransform);
                            break;
                    }
                    IncreaseSpread();
                    fireRateTimer = Time.time + fireRate;
                    currentAmmoInMag--;
                }
                else if (currentAmmoInMag == 0) //auto-reload if mag empty and player tries to fire
                {
                    BeginReloading();
                }
                break;
        }

    } //shoot action is performed
    public void Fire(Transform shooterTransform) //fires singular projectile
    {
        if (bulletPool == null)
            Debug.LogError("Bullet Pool null");
        Bullet bullet = bulletPool.GetBullet();

        if (bullet != null && !bullet.GetReadyToFire())
        {
            float angle = Random.Range(-currentRecoilSpread, currentRecoilSpread);

            if (barrelTip == null)
                Debug.Log("Barrel tip not set.");

            Vector3 bulletDirection = Quaternion.Euler(0f, angle, 0f) * barrelTip.transform.forward;

            //setting bullet properties
            bullet.SetRotationValues(Quaternion.Euler(0f, shooterTransform.rotation.eulerAngles.y - 90f, 0f));
            bullet.SetInitialPosition(barrelTipTransform);
            bullet.SetDirection(bulletDirection);
            bullet.SetVelocity(bulletVelocity);
            bullet.SetDamageAndCritValues(damage, criticalChance, criticalDamage);
            bullet.SetBulletPool(bulletPool);
            bullet.BeginDespawnTimer();
            bullet.ToggleReadyToFire(); //setting it to false to let pool know that it can't be reused currently
        }

    }


    //ammo
    public void Reload()
    {
        switch (isWeaponHandLoaded)
        {
            case false: //reload entire magazine like usual.
                if (currentStockAmmo > magazineSize)
                {
                    int temp = magazineSize - currentAmmoInMag;
                    currentAmmoInMag = magazineSize;
                    currentStockAmmo -= temp;
                }
                else if((currentAmmoInMag + currentStockAmmo) >= magazineSize) //is the stock ammo remaining greater than the mag size
                {
                    int dif = magazineSize - currentAmmoInMag;
                    currentAmmoInMag += dif; //add difference to ammo in magazine
                    currentStockAmmo -= dif; //subtract difference from stock ammo remaining
                } else //if not then use up remaining stock ammo
                {
                    currentAmmoInMag += currentStockAmmo;
                    currentStockAmmo = 0;
                }
                break;

            case true: //reload single round

                if (currentStockAmmo > roundsReloadedPerInstance && currentAmmoInMag - roundsReloadedPerInstance < magazineSize)
                {
                    currentStockAmmo -= roundsReloadedPerInstance;
                    currentAmmoInMag += roundsReloadedPerInstance;
                }
                break;
        }
    } //reloads ammunition in magazine
    public void BeginReloading()
    {
        reloadState = ReloadState.Reloading;

        currentTimer = timeToReload;
    } //starts reloading timer
    public void CancelReload()
    {
        currentTimer = 0;
        reloadState = ReloadState.Not_Reloading;
    } //stops the reload
    public void MaxAmmo()
    {
        currentAmmoInMag = magazineSize;
        currentStockAmmo = maxStockAmmo;
    } //restores players ammo


    //recoil
    public void IncreaseSpread()
    {
        currentRecoilSpread = Mathf.Clamp(currentRecoilSpread + recoilRate, 0f, recoilSpread); //increases spread within set range
    } //recoil

    //misc
    public void Drop()
    {
        weaponModel.SetActive(false);
        this.gameObject.SetActive(false);
        //Destroy(weaponModel);
        //Destroy(this.gameObject);
    } //weapon is no longer in existence after player drops

    //public void InstantiateWeapon(Transform parentT) //for when weapon gets picked up or spawned it has to be instantiated
    //{
    //    if (weaponModelPrefab != null)
    //    {
    //        GameObject weaponModelInstance = Instantiate(weaponModelPrefab, parentT);
    //        weaponModelInstance.transform.SetParent(parentT);

    //        // Add the Weapon script to the instantiated weapon model
    //        Weapon weaponScript = weaponModelInstance.GetComponent<Weapon>();
    //        if (weaponScript == null)
    //        {
    //            weaponScript = weaponModelInstance.AddComponent<Weapon>();
    //            weaponScript.weaponModel = weaponModelInstance;
    //            Debug.Log("Weapon script added to the weapon model.");
    //        }

    //        // Copy weapon properties from the current weapon to the instantiated weapon
    //        weaponScript.CopyWeaponPropertiesFrom(this);

    //        // Reset reload timers
    //        weaponScript.currentTimer = 0;

    //        // Set the spawned weapon as the active weapon in the player's inventory
    //        PlayerInventory playerInventory = parentT.GetComponent<PlayerInventory>();
    //        //playerInventory.activeWeapon = this;
    //    }
    //    else
    //    {
    //        Debug.LogError("WeaponModelPrefab does not exist.");
    //    }
    //}

    public void SetBarrelTip()
    {
        if (weaponModel == null)
            Debug.Log("Weapon Model null.");

        if (weaponModel.GetComponentInChildren<BarrelTip>() != null)
        {
            barrelTip = weaponModel.GetComponentInChildren<BarrelTip>();
            Debug.Log("barrel tip set");
        }
        else Debug.Log("Weapon don't got no Barrel tip.");
    }
    public void SetBarrelTip(BarrelTip tip)
    {
        barrelTip = tip;
    }
    public void SetWeaponModel(GameObject model)
    {
        weaponModel = model;
    }
    public void SetWeaponModel()
    {
        Transform childTransform = transform.GetChild(0); // Assuming the first child is the weapon model

        if (childTransform != null)
        {
            weaponModel = childTransform.gameObject;
        }
        else
        {
            Debug.Log("Error locating weapon model.");
        }

    }

    ////for weapon spawning
    //public void CopyWeaponPropertiesFrom(Weapon otherWeapon)
    //{
    //    weaponName = otherWeapon.weaponName;
    //    fireRate = otherWeapon.fireRate;
    //    isFullAuto = otherWeapon.isFullAuto;
    //    canToggleFullAuto = otherWeapon.canToggleFullAuto;
    //    damage = otherWeapon.damage;
    //    criticalDamage = otherWeapon.criticalDamage;
    //    criticalChance = otherWeapon.criticalChance;
    //    reloadState = otherWeapon.reloadState;
    //    isActive = otherWeapon.isActive;
    //    timeToReload = otherWeapon.timeToReload;
    //    reloadTimer = otherWeapon.reloadTimer;
    //    currentTimer = otherWeapon.currentTimer;
    //    magazineSize = otherWeapon.magazineSize;
    //    currentAmmoInMag = otherWeapon.currentAmmoInMag;
    //    currentStockAmmo = otherWeapon.currentStockAmmo;
    //    maxStockAmmo = otherWeapon.maxStockAmmo;
    //    isWeaponHandLoaded = otherWeapon.isWeaponHandLoaded;
    //    roundsReloadedPerInstance = otherWeapon.roundsReloadedPerInstance;
    //    recoilSpread = otherWeapon.recoilSpread;
    //    currentRecoilSpread = otherWeapon.currentRecoilSpread;
    //    recoilRate = otherWeapon.recoilRate;
    //    weaponType = otherWeapon.weaponType;
    //    bulletModelPrefab = otherWeapon.bulletModelPrefab;
    //    bulletVelocity = otherWeapon.bulletVelocity;
    //    // Copy any other properties you may have added
    //}

}
