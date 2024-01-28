using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private Quaternion rotation;

    //bullet pooling
    public BulletPool bulletPool;
    private bool readyToFire; //if true, bullet is ready to be fired
    public LayerMask enemyLayer;

    //physical stats
    private float velocity;
    public int penetrationHealth; //how much the bullet can penetrate
    private int currentHealth;
    private int damage;
    private float criticalChance;
    private float criticalDamage;
    public Player player; //player that fired the bullet

    //collision
    public float collisionRadius;

    private float life = 5f;
    private float despawnTime;
    // Start is called before the first frame update
    void Start()
    {
        readyToFire = true;
        //BeginDespawnTimer();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * velocity * Time.deltaTime;
        if (Time.time > despawnTime || currentHealth <= 0)
        {
            ReturnBullet();
            ToggleReadyToFire(); //should be set to true
        }

        transform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        //if (!readyToFire) //if already shot, scan for collision
        //{
            DetectCollision();
            DetectWallCollision();
        //}
    }

    //setting values
    public void SetDamageAndCritValues(int damage, float criticalChance, float criticalDamage)
    {
        this.damage = damage;
        this.criticalChance = criticalChance;   
        this.criticalDamage = criticalDamage;
        currentHealth = penetrationHealth;
    }
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }
    public void SetVelocity(float velocity)
    {
        this.velocity = velocity;
    }
    public void SetRotationValues(Quaternion rotation)
    {
        this.rotation = rotation;
    }
    public void SetInitialPosition(Transform position)
    {
        transform.position = position.position;
    }
    public void SetBulletPool(BulletPool bulletPool)
    {
        this.bulletPool = bulletPool;
        //Debug.Log("Bullet pool set");
    }
    public void SetFiringPlayer(Player player)
    {
        this.player = player;
    }

    //collision detection
    private void DetectCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius, enemyLayer); // Adjust the radius as needed
        foreach(Collider collider in hitColliders)
        {
            double adjDamage = damage * ((double)currentHealth / penetrationHealth); //adjusted damage is accounting for how much damage is dealt based on the penetration health of the bullet
            if (collider.GetComponent<Enemy>() != null)
            {
                collider.GetComponent<Enemy>().InflictDamage((int) adjDamage, player);
                currentHealth -= collider.GetComponent<Enemy>().stoppingPower;
            }
        }
    }
    private void DetectWallCollision() //collision for penetrable walls
    {

    }

    //actions
    public void Damage()
    {

    }

    //despawn
    public void BeginDespawnTimer()
    {
        despawnTime = Time.time + life;
    } //now somewhat merged with ReturnBullet

    //bullet pooling
    private void ReturnBullet()
    {
        bulletPool.ReturnBullet(this);
    }
    public void ToggleReadyToFire()
    {
        readyToFire = !readyToFire;
    }
    public bool GetReadyToFire()
    {
        return readyToFire;
    }

}
