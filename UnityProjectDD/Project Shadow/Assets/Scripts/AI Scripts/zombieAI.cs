using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Animator anim;

    public Transform player;

    public LayerMask whatIsPlayer;

    [SerializeField] bool roamingEnemy;
    [SerializeField] int roamTimer = 0;


    // patrolling (if desired; wave enemies by default but can be given roaming functionality by checking "Roaming Enemy" serialized field - if roaming enemy, can also set 'destinationRange' to 0 if you'd prefer them to be stationary when not chasing/attacking)
    public Vector3 destination;
    bool destinationSet;
    public float destinationRange;
    bool isRoamingEnemy;
    bool isWaveEnemy;

    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // states
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    public bool isEmerging;

    private Coroutine roamingCoroutine;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        isRoamingEnemy = roamingEnemy;
        isWaveEnemy = !isRoamingEnemy;
        if (isWaveEnemy) 
        { 
            destination = player.position;
            agent.SetDestination(destination); 
        }
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
            if (isRoamingEnemy)
            {
                if (!playerInSightRange && !playerInAttackRange) 
                {
                    if (roamingCoroutine == null)
                    {
                        roamingCoroutine = StartCoroutine(Roam());
                    }
                }
                else
                {
                    if (roamingCoroutine != null)
                    {
                        StopCoroutine(roamingCoroutine);
                        roamingCoroutine = null;
                    }
                }
            }
            
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
    }

    private IEnumerator Roam()
    {
        float origMovementSpeed = agent.speed;

        while (true)
        {
            if (!destinationSet) SearchDestination();

            if (destinationSet) agent.SetDestination(destination);

            while (destinationSet)
            {
                Vector3 distanceToDestination = transform.position - destination;                

                if (distanceToDestination.magnitude < 1f)
                {
                    float speed = Mathf.Lerp(agent.speed, 0, Time.deltaTime * 5);
                    agent.speed = speed;

                    if (speed < 0.01f)
                    {
                        agent.speed = 0;
                        destinationSet = false;
                        anim.SetFloat("Speed", 0);
                        agent.isStopped = true;

                        yield return new WaitForSeconds(roamTimer);

                        agent.isStopped = false;
                        agent.speed = origMovementSpeed;
                    }
                }

                yield return null;
            }
        }
    }

    private void SearchDestination()
    {
        float randomX = Random.Range(-destinationRange, destinationRange);            
        float randomZ = Random.Range(-destinationRange, destinationRange);
            
        destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, destinationRange, 1))
        {
            destination = hit.position;
            destinationSet = true;
        }
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
