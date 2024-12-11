using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntAi : MonoBehaviour
{
    // Makes a reference to the NavMeshAgent component
    private NavMeshAgent agent;

    // Player location
    private Transform player;

    // So it can tell the difference between ground and player, also neat to dodge random obstacles
    private LayerMask whatIsGround, whatIsPlayer;

    // This is patroling ai

    [SerializeField] private Vector3 walkPoint, playerTransform;
    private bool walkPointSet;
    public float walkPointRange;

    // This is attacking ai
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    // To switch between ai states
    public float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInSphereRange, playerInAttackRange;

    private void Start()
    {
        player = GameObject.Find("PlayerXROrigin").transform;
        whatIsGround = LayerMask.GetMask("groundLayer");
        whatIsPlayer = LayerMask.GetMask("playerLayer");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
       
        // Checks for the sight range and attack range in a sphere around the enemy
        playerInSphereRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Checks if the player is in the sphere range
        while (playerInSphereRange)
        {
            // If the player is in the sphere range, it will check if the player is in the sight range for actual line of sight
            playerInSightRange = Physics.Raycast(transform.position, playerTransform, whatIsPlayer);
 
            // C'mon you know what this is ya fucking ass
            Debug.DrawRay(transform.position, playerTransform, Color.red);
        }

        // Simple logic, it will either patrol, chase, or attack the player depending on what states the variables are in
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        // If the enemy doesn't have a walk point set, mostly the player, it'll search for one
        if (!walkPointSet) SearchWalkPoint();

        // NavMesh W moment
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        // This is how much space or distance or whatever there is between the enemy and the walk point
        // L: Saving this for later, just leave me be okay?
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // If the enemy gets close to the walk point it will say it doesn't have a walk point so it can search for a new one
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }       
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range, only for x and z since I don't fucking need it to fly
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // If the walk point is on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        // Will set the destination to the player's position
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Enemy will set in place once the attack range is plenty so it doesn't push into the player
        agent.SetDestination(transform.position);

        // Rotates towards the player
        transform.LookAt(player);
    }

    private void OnDrawGizmosSelected()
    {
        // L: Gizmos for the spheres, why else do you think it's called "DrawWireSphere"? And why the fuck does this stupid bot want to correct me?
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}