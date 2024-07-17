using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    public int health;
    public NavMeshAgent agent;

    public Transform player;
    public LayerMask ground, isPlayer;
    
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectile;
    

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    public float unitHealth;
    public float unitMaxHealth;
    public HealthTracker healthTracker;

    public VictoryManager victorymanager;

    public CoinsManager coinsmanager;

    public UnitSinglePlayer unitsingleplayer;
    
    public EnemySpawner spawner;


       
    public int Points { get; private set; }

    public void AddPoints(int points)
    {
        Points += points;
        Debug.Log($"Unit gained {points} points. Total points: {Points}");
    }
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        Points = 0;
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);

        coinsmanager = FindObjectOfType<CoinsManager>();
        
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
        if(!playerInAttackRange && !playerInSightRange) Patrol();
        if(!playerInAttackRange && playerInSightRange) Chasing();
        if(playerInAttackRange && playerInSightRange) Attacking();
    }
    
    private void onDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);

        if (coinsmanager != null)
        {
            coinsmanager.UpdateCoins(coinsmanager.Coins);
        }
    }
    
    private void Patrol()
    {
        if(!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
        {
            walkPointSet = true;
        }
    }
    
    private void Chasing()
    {
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {

        //empeche d'acceder a la position du player quand il est detruit
        if(player == null)
        {
            Debug.LogWarning("Player transform is null. Cannot attack");
            return;
        }


        agent.SetDestination(transform.position); // Stop moving while attacking
        transform.LookAt(player); // Face the player

        if (!alreadyAttacked)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Instantiate the projectile slightly in front of the enemy
            Vector3 spawnPosition = transform.position + directionToPlayer * 1.5f; // Adjust the offset as needed
            GameObject instantiatedProjectile = Instantiate(projectile, spawnPosition, Quaternion.identity);

            // Get the rigidbody of the projectile and set its direction
            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();
            rb.velocity = directionToPlayer * 32f; // Set the velocity directly for more consistent results

            // Set the layers for collision detection
            Projectile projectileScript = instantiatedProjectile.GetComponent<Projectile>();
            projectileScript.playerLayer = isPlayer;
            projectileScript.groundLayer = ground;

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    private void UpdateHealthUI()
    {
        if (healthTracker != null)
        {
            healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        }
        if (unitsingleplayer.unitHealth <= 0 )
        {
            /*Debug.Log("ennemi tuer");
            victorymanager.isDeadEnemy = true;
            Destroy(gameObject);*/
            HandleDeath();//CHATGPT RAJOUT
        }
    }

    private void HandleDeath()
     {
        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy died");
            victorymanager.isDeadEnemy = true;
        }
        //Destroy(gameObject);
     }
    
    

    internal void TakeDamage(int damageToInflict)
    {
        unitHealth -= damageToInflict;
        UpdateHealthUI();
    }

    private void DestroyEnemy()
    {
        
        if (spawner != null)
        {
            spawner.currentEnemy.Remove(this.gameObject);

        }
        
        Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }
}
