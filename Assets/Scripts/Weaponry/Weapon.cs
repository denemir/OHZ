using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    //info
    public string weaponName;
    public Sprite icon;

    [Header("Weapon Stats")]
    //fire rate
    public float roundsPerMinute;
    protected float timeBetweenShots;
    private float roundsPerMinuteTimer; //if != 0, can't shoot :P
    public bool isFullAuto; //should be replaced with weapon state to account for burst fire
    public bool canToggleFullAuto; //decides if weapon can fire full automatic

    //damage
    public int damage;
    public int criticalDamage;
    public float criticalChance;

    //reload && ammunition
    public float timeToReload;
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
    /// <summary>
    ///how much time is added to the weapon swap.weapon swap time works like this: 
    ///when swapping between 2 weapons, the total time to swap accounts for both weapons.smaller weapons (such as smgs
    ///and pistols, have much faster swap times.the swap time accounts for both pulling out, and putting 
    ///away. (ex., 1911 has swap value of 50. MAG-10 has swap value of 120. The total time to swap would be 170.)
    /// </summary>

    //pack a punching

    [Header("Pack-A-Punch")]
    public bool isPackAPunched;
    public PackAPunchedWeapon packAPunchVariant;

    //interactions
    public int cost; //if weapon is a wall buy, otherwise keep at 950

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

    [Header("Weapon Model")]
    //model
    public GameObject weaponModelPrefab;
    public GameObject weaponModel;

    public Transform barrelTipTransform;
    public BarrelTip barrelTip;

    [Header("Bullet Model")]
    //bullet
    public GameObject bulletModelPrefab;
    public float bulletVelocity;
    public BulletPool bulletPool;

    //player & player gui
    public Player player;
    public Character character;
    public RightHand rightHand;
    public PlayerGUIHandler playerGUIHandler;
    private bool isDoubleTapRootBeerActive;

    // Start is called before the first frame update
    void Start()
    {
        roundsReloadedPerInstance = 1;
        timeBetweenShots = 1 / (roundsPerMinute / 60f);
    }

    // Update is called once per frame
    protected void Update()
    {
        //backups for model not spawning
        if (weaponModel == null)
        {
            SetWeaponModel();
        }
        if (bulletPool == null)
            bulletPool = GetComponent<BulletPool>();

        // set where the weapon fires from
        if (barrelTip == null)
        {
            SetBarrelTip();
        }

    }

    void FixedUpdate()
    {
        DecreaseSpread(); //decrease spread over time

        //check if player holding weapon has DTRB
        if(player != null)
            isDoubleTapRootBeerActive = DoesPlayerHaveDoubleTapRootBeer();
    }

    ////weapon actions

    //shooting
    public void Shoot(Transform shooterTransform)
    {
        if (Time.time > roundsPerMinuteTimer && currentAmmoInMag > 0)
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
            currentAmmoInMag--;

            //double tap rootbeer & setting timer between shots
            if(isDoubleTapRootBeerActive)
                roundsPerMinuteTimer = Time.time + (timeBetweenShots * 0.77f); //double tap rootbeer increases rpm by 33%
            else roundsPerMinuteTimer = Time.time + timeBetweenShots;
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
            bullet.SetFiringPlayer(player);
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
                else if ((currentAmmoInMag + currentStockAmmo) >= magazineSize) //is the stock ammo remaining greater than the mag size
                {
                    int dif = magazineSize - currentAmmoInMag;
                    currentAmmoInMag += dif; //add difference to ammo in magazine
                    currentStockAmmo -= dif; //subtract difference from stock ammo remaining
                }
                else //if not then use up remaining stock ammo
                {
                    currentAmmoInMag += currentStockAmmo;
                    currentStockAmmo = 0;
                }
                break;

            case true: //reload single round

                if (currentStockAmmo >= roundsReloadedPerInstance && currentAmmoInMag - roundsReloadedPerInstance < magazineSize)
                {
                    currentStockAmmo -= roundsReloadedPerInstance;
                    currentAmmoInMag += roundsReloadedPerInstance;
                }
                else if (currentStockAmmo < roundsReloadedPerInstance && currentStockAmmo > 0 && currentAmmoInMag - roundsReloadedPerInstance < magazineSize)
                {
                    currentAmmoInMag += currentStockAmmo;
                    currentStockAmmo = 0;

                }
                break;
        }
    } //reloads ammunition in magazine
    public void MaxAmmo()
    {
        currentAmmoInMag = magazineSize;
        currentStockAmmo = maxStockAmmo;
    } //restores players ammo

    //recoil
    public void IncreaseSpread()
    {
        currentRecoilSpread = Mathf.Clamp(currentRecoilSpread + recoilRate, 0f, recoilSpread); //increases spread within set range
    } //adding spread to weapon fire
    public void DecreaseSpread()
    {
        currentRecoilSpread = Mathf.Lerp(currentRecoilSpread, 0, Time.deltaTime * 1.5f); //decrease spread over time
    } //gradual decrease after not firing

    //pack-er-punching
    public PackAPunchedWeapon GetPackAPunchVariant()
    {
        return packAPunchVariant;
    }
    public bool IsWeaponPackAPunched()
    {
        return isPackAPunched;
    }

    //model
    public void Drop()
    {
        weaponModel.SetActive(false);
        this.gameObject.SetActive(false);
        //Destroy(weaponModel);
        //Destroy(this.gameObject);
    } //weapon is no longer in existence after player drops
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

    //checks
    private bool DoesPlayerHaveDoubleTapRootBeer()
    {
        return player.GetPlayerPerks().DoesPlayerHavePerk("double tap rootbeer");
    }
}
