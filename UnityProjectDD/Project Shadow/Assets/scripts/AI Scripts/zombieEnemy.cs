using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class zombieEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;

    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    AIHealth health;

    [SerializeField] int HP;
    [SerializeField] int maxHP;
    [SerializeField] float attackRate;

    bool isAttacking;
    bool targetInRange;
    bool destChosen;

    float angleToTarget;
    float stoppingDistOrig;

    List<GameObject> potentialTargets = new List<GameObject>();
    GameObject selectedTarget;

    Vector3 targetDir;
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

        if (targetInRange && !canSeeTarget())
        {
            if (!destChosen)
            {
                StartCoroutine(roam());
            }
        }
        else if (!targetInRange)
        {
            if (!destChosen)
            {
                StartCoroutine(roam());
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Raider"))
        {
            if (!potentialTargets.Contains(other.gameObject))
            {
                potentialTargets.Add(other.gameObject);
            }

            targetInRange = potentialTargets.Count > 0;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Raider"))
        {
            if (potentialTargets.Contains(other.gameObject))
            {
                potentialTargets.Remove(other.gameObject);
            }

            targetInRange = potentialTargets.Count > 0;
            if (potentialTargets.Count == 0)
            {
                agent.stoppingDistance = 0;
            }
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
        Destroy(gameObject);
        gameManager.instance.updateGameGoal(-1);
    }

    bool canSeeTarget()
    {
        GameObject closestTarget = selectTarget();
        if (closestTarget == null)
        {
            return false;
        }

        selectedTarget = closestTarget;

        targetDir = selectedTarget.transform.position - headPos.position;
        angleToTarget = Vector3.Angle(new Vector3(targetDir.x, targetDir.y + 1, targetDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, targetDir, out hit))
        {
            if ((hit.collider.CompareTag("Player") || hit.collider.CompareTag("Raider")) && angleToTarget < viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(selectedTarget.transform.position);

                //if (!isAttacking && HP > 0)
                //{

                //}

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


        yield return new WaitForSeconds(attackRate);
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    GameObject selectTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject target in potentialTargets)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}
