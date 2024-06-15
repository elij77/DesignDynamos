using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class zombieEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Transform attackPosRight;
    [SerializeField] Transform attackPosLeft;

    [SerializeField] int viewAngle;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    AIHealth health;

    [SerializeField] int HP;
    [SerializeField] int maxHP;
    [SerializeField] int attackDmg;
    [SerializeField] float attackRate;
    [SerializeField] float attackDist;

    bool isAttacking;
    bool playerInRange;
    bool destChosen;

    float angleToPlayer;
    float stoppingDistOrig;
    float lastAttackTime;
    int enemyTemp;

    Vector3 playerDir;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        

        health = GetComponentInChildren<AIHealth>();
        health.updateHealthBar(HP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));
        

        if (playerInRange && !canSeePlayer())
        {           
            if (!destChosen)
            {
                StartCoroutine(roam());
            }
        }
        else if (!playerInRange)
        {    
            if (!destChosen)
            {
                StartCoroutine(roam());
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;            
        }
    }

    public void takeDamage(int amount)
    {
        anim.SetTrigger("TakeDamage");
        HP -= amount;

        health.updateHealthBar(HP, maxHP);

        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            Vector3 stop = Vector3.zero;
            agent.velocity = stop;
            agent.acceleration = 0;
            anim.SetTrigger("Death");
        }
    }

    public void death()
    {
        enemyTemp = gameManager.instance.GetEnemyCount() - 1;
        Destroy(gameObject);

        gameManager.instance.updateEnemyGoal(enemyTemp);

        gameManager.instance.updateEnemyGoal(-1);
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, playerDir, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isAttacking && HP > 0 && agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                    StartCoroutine(attack());
                    lastAttackTime = Time.time;
                    
                }

                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    IEnumerator roam()
    {
        if(!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;

        Vector3 playerLocation = (gameManager.instance.player.transform.position - headPos.position).normalized;
        attackPosRight.rotation = Quaternion.LookRotation(playerLocation);
        attackPosLeft.rotation = Quaternion.LookRotation(playerLocation);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }

    public void createSwingRay()
    {
        RaycastHit hitRight;
        RaycastHit hitLeft;

        if (Physics.Raycast(attackPosRight.position, attackPosRight.forward, out hitRight, attackDist))
        {
            if (hitRight.collider.CompareTag("Player"))
            {
                IDamage target = hitRight.collider.GetComponent<IDamage>();
                if (target != null)
                {
                    target.takeDamage(attackDmg);
                }
            }
        }
        else if (Physics.Raycast(attackPosLeft.position, attackPosLeft.forward, out hitLeft, attackDist))
        {
            if (hitLeft.collider.CompareTag("Player"))
            {
                IDamage target = hitLeft.collider.GetComponent<IDamage>();
                if (target != null)
                {
                    target.takeDamage(attackDmg);
                }
            }
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }
}
