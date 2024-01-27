using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //nav mesh
    public NavMeshAgent agent;
    public Transform target;

    //stats
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public float attackRate; //time between attacks
    public int killPointReward;
    public int damagePointReward;

    //attack variables
    public float attackDistance;
    private Player attackingPlayer;

    //interactions
    public void InflictDamage(int damage, Player player)
    {
        attackingPlayer = player;
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

    //movement
    public void MoveTowardsTarget(float speed)
    {
        agent.speed = speed;
        agent.SetDestination(target.position);
    }
    public void RemainIdle()
    {

    }
    public void Wander()
    {

    }

    //path finding
    public Transform FindNearestPlayer()
    {
        return null;
    }
    public Transform TargetNearestPlayer()
    {
        return null;
    }
    public Transform TargetAttackingPlayer()
    {
        if(attackingPlayer != null)
            return attackingPlayer.transform;
        return null;
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
    private void DropPowerup()
    {

    }
    public void DropPowerup(GameObject powerupPrefab)
    {

    }
    private void ResetStats()
    {
        currentHealth = maxHealth;
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
