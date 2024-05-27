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


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);

        health = GetComponentInChildren<AIHealth>();

        health.updateHealthBar(HP, maxHP);

        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
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
        else if (!isShooting)
        {
            StartCoroutine(shoot());
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

        Instantiate(bullet, shootPos.position, shootPos.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        health.updateHealthBar(HP, maxHP);

        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashred());

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }


    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting)
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
        model.material.color = Color.blue;
    }
}