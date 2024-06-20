using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Animator anim;

    public Transform player;

    public LayerMask whatIsPlayer;


    // patrolling (if desired; can also just set 'destinationRange' to 0 if you'd prefer them to be stationary when not chasing/attacking)
    public Vector3 destination;
    bool destinationSet;
    public float destinationRange;

    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // states
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    public bool isEmerging;



    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), Mathf.Abs(animSpeed), Time.deltaTime));

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!isEmerging)
        {
            if (!playerInSightRange && !playerInAttackRange) Roam();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
    }

    private void Roam()
    {
        if (!destinationSet) SearchDestination();

        if (destinationSet) agent.SetDestination(destination);

        Vector3 distanceToDestination = transform.position - destination;

        if (distanceToDestination.magnitude < 1f) destinationSet = false;
    }

    private void SearchDestination()
    {
            float randomX = Random.Range(-destinationRange, destinationRange);
            float randomZ = Random.Range(-destinationRange, destinationRange);

            destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            NavMeshHit hit;
            NavMesh.SamplePosition(destination, out hit, destinationRange, 1);          
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // attack code goes here
            // 1: play attack animation
            //anim.SetTrigger("Attack");

            // 2: detect if player is in range of attack


            // 3: damage player




            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void StopMovement()
    {
        agent.isStopped = true;
        isEmerging = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false;
        isEmerging = false;
    }
}
