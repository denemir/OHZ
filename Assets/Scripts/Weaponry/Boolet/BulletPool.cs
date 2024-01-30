using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //vars
    public GameObject bulletPrefab;
    public int poolSize;
    public int poolLimit;

    private List<Bullet> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBulletPool();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //initialization
    public void InitializeBulletPool()
    {
        bulletPool = new List<Bullet>();

        for (int i = 0; i < poolSize; i++)
        {
            AddBulletToPool();
        }

        poolLimit = 100; //for testing
    }

    //pull unused bullet
    public Bullet GetBullet()
    {

        foreach (Bullet bullet in bulletPool)
        {
            if (bulletPool.Count(bullet => bullet.gameObject.activeInHierarchy) >= poolSize)
            {
                ExpandBulletPool();
                bulletPool.Last().gameObject.SetActive(true);
                return bulletPool.Last(); //return most recently created bullet
            }
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }

        }

        //all bullets are currently in use
        return null;
    }

    //return bullet that is no longer in use
    public void ReturnBullet(Bullet bullet)
    {
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(false);

        //Debug.Log("Bullet returned, available bullets: " + (bulletPool.Count() - bulletPool.Count(bullet => bullet.gameObject.activeInHierarchy)));
    }

    //in the case that fire rate is exceptional and requires more bullets, expand bulletPool up to limit
    public void ExpandBulletPool()
    {
        //determine how many bullets are currently active
        int activeBullets = bulletPool.Count(bullet => bullet.gameObject.activeInHierarchy);

        if (bulletPool.Count < poolLimit)
            AddBulletToPool();
        //Debug.Log("Expanding bullet pool to " + bulletPool.Count());
    }

    //add individual bullet to pool
    private void AddBulletToPool()
    {
        //adding bullets to pool
        GameObject bulletInstance = Instantiate(bulletPrefab);
        Bullet bullet = bulletInstance.GetComponent<Bullet>();
        bulletPool.Add(bullet);
        bullet.gameObject.SetActive(false);
    }

    //when bullet fires from weapon, it should come from the current weapon barrel tip
    //public void FireBullet()
    //{

    //}
}
