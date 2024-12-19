using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultAi : MonoBehaviour
{
    [SerializeField] private Transform playerLocation;

    [SerializeField] private LayerMask whatIsPlayer, whatIsGround;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float speed, attackRange;

    public float health;

    private bool playerInAttackRange = false;

    public void Start()
    {
        playerLocation = GameObject.Find("PlayerXROrigin").transform;
        whatIsGround = LayerMask.GetMask("groundLayer");
        whatIsPlayer = LayerMask.GetMask("playerLayer");
        agent = GetComponent<NavMeshAgent>();

        speed = 5f;
        health = 20f;
        attackRange = 4;
        agent.speed = speed;

        // L: So apparently this is how you start a coroutine
        // L: You don't do it in the Update, but in the Start, what the fuck why did nobody tell me this
        StartCoroutine(EnemyActions(0.3f));
    }

    IEnumerator EnemyActions(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            if (playerInAttackRange)
            {
                AttackRange();
                Debug.Log("Player is in attack range");
            }
            else
            {
                MoveToPlayer();
                Debug.Log("Chasing the player doo do doo do dooooo do");
            }

            DeathCheck();
            Debug.Log("Enemy action is getting called"); 
        }
    }

    public void AttackRange()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        agent.SetDestination(transform.position);
        transform.LookAt(playerLocation);
    }
    public void MoveToPlayer()
    {
        agent.SetDestination(playerLocation.position);
    }

    public void DeathCheck()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
