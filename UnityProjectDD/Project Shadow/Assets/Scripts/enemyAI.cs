using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject bullet;

    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    AIHealth health;


    [SerializeField] int HP;
    [SerializeField] int maxHP;
    [SerializeField] float shootRate;

    
    
    bool isShooting;
    bool playerInRange;
    bool destChosen;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
    float stoppingDistOrig;

    //placeholder for money
    ulong place;

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
            //agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
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

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Vector3 playerLocation = (gameManager.instance.player.transform.position - shootPos.position).normalized;

        shootPos.rotation = Quaternion.LookRotation(playerLocation);

        anim.SetTrigger("Shoot");
        

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {
        anim.SetTrigger("TakeDamage");
        HP -= amount;
        if (HP >= 0)
        {
            place = (ulong)amount * 10;
            gameManager.instance.updatePoints(place);
        }
        health.updateHealthBar(HP, maxHP);
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashred());

        if (HP <= 0)
        {
            Vector3 stop = Vector3.zero;
            agent.velocity = stop;
            agent.acceleration = 0;
            anim.SetTrigger("Death");
        }
    }

    //IEnumerator death()
    //{
    //    isShooting = false;
    //    gameManager.instance.updateGameGoal(-1);
    //    anim.SetTrigger("Death");

    //    yield return new WaitForSeconds(5);
    //    Destroy(gameObject);
    //}

    public void death()
    {
        Destroy(gameObject);
        gameManager.instance.updateGameGoal(-1);
        place = 100;
        gameManager.instance.updatePoints(place);
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);
        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting && HP > 0)
                {
                    StartCoroutine(shoot());
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }



    IEnumerator flashred()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;

        //float duration = 0.5f;
        //float elapsed = 0f;

        //model.material.color = Color.red;

        //while (elapsed < duration)
        //{
        //    elapsed += Time.deltaTime;

        //    float fraction = elapsed / duration;

        //    model.material.color = Color.Lerp(Color.red, Color.white, fraction);

        //    yield return null;
        //}

        //model.material.color = Color.white;
    }
}