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
    public int killPointReward;
    public int damagePointReward;

    //attack variables
    public float attackDistance;

    //interactions
    public void InflictDamage(int damage, Player player)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            RewardPlayer(player, killPointReward);
        }
        RewardPlayer(player, damagePointReward);           
    }

    //spawn
    public void Spawn()
    {
        SpawnAnimation();
    }

    //path finding
    private void FindNearestPlayer()
    {

    }
    private void TargetNearestPlayer()
    {

    }
    private void TargetAttackingPlayer()
    {

    }
    private void TargetBarricade()
    {

    }

    //attack player

    //death
    private void Die() //death is unfortunately inevitable. maybe not for the enemies tho
    {
        DeathAnimation();
        gameObject.SetActive(false);
    }
    private void RewardPlayer(Player player, int amount)
    {
        player.playerStats.AddPoints(amount);
    }

    //animations
    private void SpawnAnimation()
    {

    }
    private void DeathAnimation()
    {

    }
    private void WalkAnimation()
    {

    }
    private void RunAnimation()
    {

    }
    private void AttackAnimation()
    {

    }
}
