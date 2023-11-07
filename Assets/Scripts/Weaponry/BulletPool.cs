using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //stats
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

        for(int i = 0; i < poolSize; i++) 
        {
            AddBulletToPool();
        }
    }

    //pull unused bullet
    public Bullet GetBullet()
    {
        foreach(Bullet bullet in bulletPool)
        {
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
        bullet.gameObject.SetActive(false);
    }

    //in the case that fire rate is exceptional and requires more bullets, expand bulletPool up to limit
    public void ExpandBulletPool()
    {
        if (poolSize < poolLimit)
            AddBulletToPool();
            
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
