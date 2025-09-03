using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    //weapon info
    public string weaponName;
    public Sprite icon;

    //weapon stats
    public int damage;
    public float speed;
    public float range;
    public float meleeAngle; //angle of mesh

    //melee mesh
    private Transform meleeMesh;

    //tracking
    public LineRenderer trajectory;
    public LayerMask enemyLayer;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //preparing for attack
    private void CreateMeleeMesh()
    {
        meleeMesh = new GameObject("MeleeRange").transform;
        meleeMesh.SetParent(transform); //attach to player
        meleeMesh.localPosition = new Vector3(0, 0.5f, 1f) * range; //push interaction zone in front of player
    }
    private Enemy IsEnemyWithinRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(meleeMesh.position, range, enemyLayer); //detect enemies on the enemy layer
        return null;
    }

    //attack (if enemy is confirmed within swing)
    private void DealDamage()
    {

    }
}
