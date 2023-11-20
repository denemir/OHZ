using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private Quaternion rotation;

    //bullet pooling
    public BulletPool bulletPool;
    private bool readyToFire; //if true, bullet is ready to be fired

    //physical stats
    private float velocity;
    private float damage;
    private float criticalChance;
    private float criticalDamage;

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
        if (Time.time > despawnTime)
        {
            ReturnBullet();
            ToggleReadyToFire(); //should be set to true
        }
        transform.rotation = rotation;
    }

    //setting values
    public void SetDamageAndCritValues(float damage, float criticalChance, float criticalDamage)
    {
        this.damage = damage;
        this.criticalChance = criticalChance;   
        this.criticalDamage = criticalDamage;  
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
        Debug.Log("Bullet pool set");
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
