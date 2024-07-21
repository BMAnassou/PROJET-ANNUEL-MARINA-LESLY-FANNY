using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMulti : MonoBehaviour
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

    //public VictoryManager victorymanager;

    //public CoinsManager coinsmanager;

    public Unit unit;

    public EnemySpawnerMulti spawner;

    public int Points { get; private set; }

    public void AddPoints(int points)
    {
        Points += points;
        Debug.Log($"Unit gained {points} points. Total points: {Points}");
    }

    private void Awake()
    {
        Debug.Log("Awake started");

        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure there is a GameObject tagged 'Player' in the scene.");
        }
        else
        {
            Debug.Log("Player found");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on enemy!");
        }
        else
        {
            Debug.Log("NavMeshAgent component found");
        }

        Debug.Log("Awake completed");
    }

    void Start()
    {
        Points = 0;
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);

       /* coinsmanager = FindObjectOfType<CoinsManager>();
        if (coinsmanager == null)
        {
            Debug.LogError("CoinsManager not found in the scene.");
        }
        else
        {
            Debug.Log("CoinsManager found");
        }*/
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
        if (!playerInAttackRange && !playerInSightRange) Patrol();
        if (!playerInAttackRange && playerInSightRange) Chasing();
        if (playerInAttackRange && playerInSightRange) Attacking();
    }

    private void OnDestroy()
    {
        Debug.Log("Enemy OnDestroy called");

        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);

        /*if (coinsmanager != null)
        {
            coinsmanager.UpdateCoins(coinsmanager.Coins);
        }
        else
        {
            Debug.LogWarning("CoinsManager is null!");
        }*/

        if (spawner != null)
        {
            Debug.Log("Notifying spawner of enemy destruction");
            spawner.OnEnemyDestroyed(gameObject);
        }
        else
        {
            Debug.LogWarning("Spawner is null!");
        }
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
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
        if (player == null)
        {
            Debug.LogWarning("Player transform is null. Cannot attack");
            return;
        }

        agent.SetDestination(transform.position); // Stop moving while attacking
        transform.LookAt(player); // Face the player

        if (!alreadyAttacked)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            Vector3 spawnPosition = transform.position + directionToPlayer * 1.5f;
            GameObject instantiatedProjectile = Instantiate(projectile, spawnPosition, Quaternion.identity);

            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();
            rb.velocity = directionToPlayer * 32f;

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
        if (unit.unitHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy died");
            //victorymanager.isDeadEnemy = true;
        }
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

    public void SetSpawner(EnemySpawnerMulti _spawner)
    {
        spawner = _spawner;
    }
}
