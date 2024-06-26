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
    public Collider hitCollider;

    // circling (strafing)
    int strafeMovementRange; // how far enemy will move when strafing
    int strafeDistance; // distance from player that enemy will strafe
    bool willStrafe;
    bool strafeDirection; // true: right, false: left

    // states
    public float sightDistance;
    public float attackDistance;
    public bool playerInSightDistance;
    public bool playerInAttackDistance;
    public bool playerInStrafeDistance;
    public bool isEmerging;

    private Coroutine roamingCoroutine;

    long place;
    bool isDead = false;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        isRoamingEnemy = roamingEnemy;
        isWaveEnemy = !isRoamingEnemy;
        willStrafe = Random.value < 0.5f;
        if (isWaveEnemy) 
        { 
            destination = player.position;
            agent.SetDestination(destination); 
        }
        if (willStrafe)
        {
            strafeMovementRange = Random.Range(5, 25);
            strafeDistance = Random.Range(5, 15);
            strafeDirection = Random.value < 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.playerHPBar.fillAmount == 0)
            
        {
            return;
        }
        else
        {
            float animSpeed = agent.velocity.normalized.magnitude;
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), Mathf.Abs(animSpeed), Time.deltaTime));
            if (HP > 0)
            {
                anim.SetInteger("HP", 1);
            }else if (HP <= 0) 
            {
                anim.SetInteger("HP", 0);
            }

            playerInSightDistance = Physics.CheckSphere(transform.position, sightDistance, whatIsPlayer);
            playerInAttackDistance = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer);
            playerInStrafeDistance = Vector3.Distance(transform.position, player.position) <= strafeDistance;

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
                else if (!isRoamingEnemy)
                {
                    destination = player.position;
                    agent.SetDestination(destination);
                }

                if (playerInSightDistance && !playerInAttackDistance) ChasePlayer();
                if (playerInSightDistance && playerInAttackDistance) StartCoroutine(PerformAttack());
                if (willStrafe && playerInStrafeDistance) CirclePlayer();
                
            }
        
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

    private void CirclePlayer()
    {
        Vector3 strafeDirectionVector = strafeDirection ? transform.right : -transform.right;
        Vector3 strafePosition = transform.position + strafeDirectionVector * strafeMovementRange;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(strafePosition, out hit, strafeMovementRange, 1))
        {
            agent.SetDestination(hit.position);
        }

        agent.SetDestination(player.position);
    }

    IEnumerator PerformAttack()
    {
        if (alreadyAttacked)
        {
            yield break;
        }

        
        alreadyAttacked = true;
        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(playerPos);
        if (HP > 0)
        {
            anim.SetTrigger("Attack");
            AudioManager.Instance.playSFX("Zombie Attack");
            Collider[] hitPlayer = Physics.OverlapSphere(attackPos.position, attackRange, whatIsPlayer);
            foreach (Collider hit in hitPlayer)
            {
                IDamage dmg = hit.GetComponent<IDamage>();
                if (dmg != null)
                {
                    yield return new WaitForSeconds(0.5f);
                    dmg.takeDamage(attackDmg);
                }

            }
            yield return new WaitForSeconds(timeBetweenAttacks);
        }


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

    private void OnDrawGizmosSelected()
    {
        if (attackPos == null)
            return;

        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void takeDamage(int amount)
    {
        if (HP > 0)
        {
            if (!isDead)
            {
                AudioManager.Instance.playSFX("Zombie Hit");
                anim.SetTrigger("TakeDamage");
                HP -= amount;
                //StartCoroutine(FlashRed());
                agent.SetDestination(player.position);
            }
            
        }
        
        if (HP >= 0)
        {
            if (!isDead)
            {
                place = amount * 5;
                gameManager.instance.updatePoints(place);
            }
            
        }

        

        if (HP <= 0)
        {
            isDead = true;
            Vector3 stop = Vector3.zero;
            agent.velocity = stop;
            agent.acceleration = 0;
            anim.SetTrigger("Death");
            Destroy(hitCollider);
            anim.SetBool("isDead", true);
        }
    }

    public void death()
    {
        AudioManager.Instance.playSFX("Zombie Death");

        Destroy(gameObject);

        gameManager.instance.updateEnemyGoal(-1);
        place = 100;
        gameManager.instance.updatePoints(place);
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
