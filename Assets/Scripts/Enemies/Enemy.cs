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
    public int stoppingPower; //how much a bullet can penetrate thru

    //point awarding
    public int pointAwardCounter;
    public int pointAwardLimit;

    //attack variables
    private Player attackingPlayer;

    //interactions
    public void InflictDamage(int damage, Player player)
    {
        attackingPlayer = player;
        currentHealth -= damage;
        RewardPlayer(player, damagePointReward);
        if (currentHealth <= 0)
        {
            Die();
            RewardPlayerForKill(player, killPointReward);
        }
    }
    //public void TakeDamage(int damage, Player player)
    //{
    //    currentHealth -= damage;
    //    if (player != null)
    //        RewardPlayer(player, damagePointReward);

    //    //if enemy dies
    //    if (currentHealth <= 0)
    //    {
    //        if (player != null)
    //            RewardPlayer(player, killPointReward);
    //        Die();
    //    }
    //}

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
        if (attackingPlayer != null)
            return attackingPlayer.transform;
        return null;
    }

    //attack player

    //death
    protected void Die() //death is unfortunately inevitable. maybe not for the enemies tho
    {
        DeathAnimation();
        gameObject.SetActive(false);
    }
    protected void RewardPlayer(Player player, int amount)
    {
        if (pointAwardCounter < pointAwardLimit)
        {
            player.playerStats.AddPoints(amount);
            pointAwardCounter++;
        }
    }
    protected void RewardPlayerForKill(Player player, int amount) //guaranteed award for kill
    {
        player.playerStats.AddPoints(amount);
    }
    protected void DropPowerup()
    {

    }
    public void DropPowerup(GameObject powerupPrefab)
    {

    }
    protected void ResetStats()
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
