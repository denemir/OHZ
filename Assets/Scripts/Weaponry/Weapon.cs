using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public string weaponName;

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

    //player & player gui
    public Player player;
    public Character character;
    public RightHand rightHand;
    public PlayerGUIHandler playerGUIHandler;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateWeapon();
        roundsReloadedPerInstance = 1;

        //if (playerGUIHandler == null)
        //{
        //    Transform currentTransform = transform; // Start from the Weapon's transform
        //                                            // Keep moving up the hierarchy until we either find the PlayerGUIHandler
        //                                            // or reach the top level (null parent).
        //    while (currentTransform != null)
        //    {
        //        playerGUIHandler = currentTransform.GetComponent<PlayerGUIHandler>();
        //        if (playerGUIHandler != null)
        //        {
        //            // We found the PlayerGUIHandler, so we can exit the loop.
        //            break;
        //        }
        //        currentTransform = currentTransform.parent;
        //    }
        //} //gui
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimer > 0)        
            currentTimer -= 0.005f;
        

        if (currentTimer <= 0 && reloadState == ReloadState.Reloading)
        {
            Reload();

            //in case of hand load
            if (isWeaponHandLoaded && currentAmmoInMag < magazineSize)
            {
                BeginReloading();
            } else reloadState = ReloadState.Not_Reloading;
            
            //playerGUIHandler.UpdateCurrentAmmoInWeapon();
            //playerGUIHandler.UpdateCurrentStockAmmo();
        } //reload

        //weaponModel.transform.position = transform.position;
        currentRecoilSpread = Mathf.Lerp(currentRecoilSpread, 0, Time.deltaTime * 1.5f); //decrease spread over time

    }
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
                    //playerGUIHandler.UpdateCurrentAmmoInWeapon(); //update player gui
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

        float angle = Random.Range(-currentRecoilSpread, currentRecoilSpread);

        Vector3 bulletDirection = Quaternion.Euler(0f, angle, 0f) * barrelTip.transform.forward;

        GameObject bullet = Instantiate(bulletModelPrefab, barrelTip.transform.position, Quaternion.identity/*Quaternion.LookRotation(bulletDirection)*//*new Quaternion(barrelTip.transform.rotation.x, barrelTip.transform.rotation.y, barrelTip.transform.rotation.z, 0f)*/);
        bullet.GetComponent<Bullet>().SetRotationValues(Quaternion.Euler(0f, shooterTransform.rotation.eulerAngles.y - 90f, 0f));

        bullet.GetComponent<Bullet>().SetDirection(bulletDirection);
        bullet.GetComponent<Bullet>().SetVelocity(bulletVelocity);
        bullet.GetComponent<Bullet>().SetDamageAndCritValues(damage, criticalChance, criticalDamage);
    }

    public void IncreaseSpread()
    {
        currentRecoilSpread = Mathf.Clamp(currentRecoilSpread + recoilRate, 0f, recoilSpread); //increases spread within set range
    } //recoil

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
                else
                {
                    currentAmmoInMag = currentStockAmmo;
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

    public void Drop()
    {
        Destroy(weaponModel);
        Destroy(this.gameObject);
    } //weapon is no longer in existence after player drops

    public void MaxAmmo()
    {
        currentAmmoInMag = magazineSize;
        currentStockAmmo = maxStockAmmo;
    } //restores players ammo

    public void CancelReload()
    {
        currentTimer = 0;
        reloadState = ReloadState.Not_Reloading;
    } //stops the reload

    public void InstantiateWeapon() //for when weapon gets picked up or spawned it has to be instantiated
    {
        //weapon instantiation
        weaponModel = Instantiate(weaponModelPrefab, transform);

        //set barrel tip
        if (weaponModel.GetComponentInChildren<BarrelTip>() != null)
        {
            barrelTip = weaponModel.GetComponentInChildren<BarrelTip>();
        }
        else Debug.Log("Weapon don't got no Barrel tip.");

        //reloads
        currentTimer = 0;
    }
    public void InstantiateWeapon(Transform parent)
    {
        //weapon instantiation
        weaponModel = Instantiate(weaponModelPrefab, parent);

        //set barrel tip
        if (weaponModel.GetComponentInChildren<BarrelTip>() != null)
        {
            barrelTip = weaponModel.GetComponentInChildren<BarrelTip>();
        }
        else Debug.Log("Weapon don't got no Barrel tip.");

        //reloads
        currentTimer = 0;
    }
}
