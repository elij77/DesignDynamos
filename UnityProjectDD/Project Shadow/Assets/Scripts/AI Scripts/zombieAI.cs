using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour, IDamage
{
    public LayerMask whatIsPlayer;

    public NavMeshAgent agent;

    public Animator anim;

    public Transform player;
    public Transform attackPos;

    [SerializeField] Renderer model;
    [SerializeField] bool roamingEnemy;
    [SerializeField] int roamTimer = 0;
    //[SerializeField] int facePlayerSpeed = 5;

    [SerializeField] int HP;
    [SerializeField] int maxHP;


    // patrolling (if desired; wave enemies by default but can be given roaming functionality by checking "Roaming Enemy" serialized field - if roaming enemy, can also set 'destinationRange' to 0 if you'd prefer them to be stationary when not chasing/attacking)
    public Vector3 destination;
    bool destinationSet;
    public float destinationRange;
    bool isRoamingEnemy;
    bool isWaveEnemy;

    // attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] int attackDmg = 2;

    // states
    public float sightDistance;
    public float attackDistance;
    public bool playerInSightDistance;
    public bool playerInAttackDistance;
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

        playerInSightDistance = Physics.CheckSphere(transform.position, sightDistance, whatIsPlayer);
        playerInAttackDistance = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer);

        if (!isEmerging)
        {
            if (isRoamingEnemy)
            {
                if (!playerInSightDistance && !playerInAttackDistance) 
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
            
            if (playerInSightDistance && !playerInAttackDistance) ChasePlayer();
            if (playerInSightDistance && playerInAttackDistance) StartCoroutine(PerformAttack());
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

    //private void AttackPlayer()
    //{
    //    agent.SetDestination(transform.position);

    //    Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
    //    transform.LookAt(playerPos);

    //    if (!alreadyAttacked)
    //    {
    //        // attack code goes here
    //        // 1: play attack animation
    //        anim.SetTrigger("Attack");
    //        alreadyAttacked = true;
    //        StartCoroutine(PerformAttack());
    //    }
    //}

    //IEnumerator PerformAttack()
    //{
    //    //AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
    //    //yield return new WaitForSeconds(animStateInfo.length);

    //    Collider[] hitPlayer = Physics.OverlapSphere(attackPos.position, attackRange, whatIsPlayer);

    //    foreach (Collider player in hitPlayer)
    //    {
    //        IDamage dmg = player.GetComponent<IDamage>();
    //        if (dmg != null)
    //        {
    //            dmg.takeDamage(attackDmg);
    //        }
    //    }

    //    yield return new WaitForSeconds(timeBetweenAttacks);
    //    ResetAttack();
    //}

    //void ResetAttack()
    //{
    //    alreadyAttacked = false;
    //}

    IEnumerator PerformAttack()
    {
        //alreadyAttacked = true;
        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(playerPos);
        anim.SetTrigger("Attack");
        AudioManager.Instance.playSFX("Zombie Attack");
        Collider[] hitPlayer = Physics.OverlapSphere(attackPos.position, attackRange, whatIsPlayer);
        foreach (Collider hit in hitPlayer)
        {
            IDamage dmg = hit.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(attackDmg);
            }
        }

        yield return new WaitForSeconds(timeBetweenAttacks);
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

    private void OnDrawGizmosSelected()
    {
        if (attackPos == null)
            return;

        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void takeDamage(int amount)
    {
        AudioManager.Instance.playSFX("Zombie Hit");
        anim.SetTrigger("TakeDamage");
        HP -= amount;

        StartCoroutine(FlashRed());

        if (HP <= 0)
        {
            agent.isStopped = true;
            anim.SetTrigger("Death");
        }
    }

    public void death()
    {
        AudioManager.Instance.playSFX("Zombie Death");

        Destroy(gameObject);

        gameManager.instance.updateEnemyGoal(-1);
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
