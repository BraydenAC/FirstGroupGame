using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float sightRadius = 10f;       // Sight radius of the enemy
    public float attackRadius = 5f;       // Attack radius of the enemy
    public float moveSpeed = 3f;          // Movement speed of the enemy

    public int maxHealth = 100;
    
    [SerializeField]
    private int currentHealth;

    private Transform player;             // Reference to the player's transform
    private Vector3 patrolDestination;    // Destination for patrolling
    private bool isChasing = false;       // Flag for if enemy is chasing the player

    private enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }

    private EnemyState currentState;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Patrolling;
        GeneratePatrolDestination();

        // Initialize currentHealth to maxHealth
        currentHealth = maxHealth;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();

                if (distanceToPlayer <= sightRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                Chase();

                if (distanceToPlayer > sightRadius)
                {
                    currentState = EnemyState.Patrolling;
                    GeneratePatrolDestination();
                }
                else if (distanceToPlayer <= attackRadius)
                {
                    currentState = EnemyState.Attacking;
                }
                break;

            case EnemyState.Attacking:
                Attack();

                if (distanceToPlayer > attackRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
        }
    }

    private void Patrol()
    {
        Debug.Log("Patrol");

        // Move towards the patrol destination
        transform.position += (patrolDestination - transform.position).normalized * moveSpeed * Time.deltaTime;

        // If the enemy is close to the patrol destination, generate a new patrol destination
        if (Vector3.Distance(transform.position, patrolDestination) < 0.1f)
        {
            GeneratePatrolDestination();
        }
    }

    private void Chase()
    {
        Debug.Log("Chase!");

        // Calculate the movement direction towards the player's position
        Vector3 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        // Attack goes here
        Debug.Log("Attack!");
    }

    private void GeneratePatrolDestination()
    {
        // Generate a random patrol destination within a range
        Vector2 randomCircle = Random.insideUnitCircle * sightRadius;
        patrolDestination = new Vector3(randomCircle.x, transform.position.y, randomCircle.y);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if the enemy's health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
