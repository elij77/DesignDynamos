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
    //[SerializeField] Transform attackPosRight;
    //[SerializeField] Transform attackPosLeft;

    [SerializeField] int viewAngle;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    

    [SerializeField] int HP;
    [SerializeField] int maxHP;
    [SerializeField] int attackDmg;
    [SerializeField] float attackRate;
    [SerializeField] float attackDist;

    bool isAttacking;
    //bool playerInRange;
    bool destChosen;

    float angleToPlayer;
    float stoppingDistOrig;
    float lastAttackTime;
    int enemyTemp;

    Vector3 playerDir;
    Vector3 startingPos;

    SphereCollider attackCol;

    long place;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        attackCol = GetComponent<SphereCollider>();

        
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        agent.SetDestination(gameManager.instance.player.transform.position);

        //if (playerInRange && !canSeePlayer())
        //{           
        //    if (!destChosen)
        //    {
        //        StartCoroutine(roam());
        //    }
        //}
        //else if (!playerInRange)
        //{    
        //    if (!destChosen)
        //    {
        //        StartCoroutine(roam());
        //    }
        //}

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isAttacking && HP > 0 )
        {
                            faceTarget();
                            StartCoroutine(attack());
                            lastAttackTime = Time.time;
            

        }
        //StartCoroutine(attack());
        if (other.isTrigger)
            return;

        if (other.CompareTag("Zombie"))
            return;

        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(attackDmg);
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInRange = false;
            agent.stoppingDistance = 0;            
        }
    }

    void enableAttack()
    {
  
      attackCol.enabled = true;
  
    }

    void disableAttack()
    {
        attackCol.enabled = false;
    }

    public void takeDamage(int amount)
    {
        AudioManager.Instance.playSFX("Zombie Hit");
        anim.SetTrigger("TakeDamage");
        HP -= amount;

        

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
        AudioManager.Instance.playSFX("Zombie Death");
        Destroy(gameObject);

        gameManager.instance.updateEnemyGoal(-1);

        place = 100;
        gameManager.instance.updatePoints(place);

        
    }

    

    IEnumerator attack()
    {
        isAttacking = true;
        AudioManager.Instance.playSFX("Zombie Attack");
        Vector3 playerLocation = (gameManager.instance.player.transform.position - headPos.position).normalized;
        
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
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
