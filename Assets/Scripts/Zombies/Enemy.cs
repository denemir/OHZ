using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    //stats
    public int maxHealth;
    public int currentHealth;

    public int damage;

    public float attackRate; //time between attacks

    public float moveSpeed;
}
