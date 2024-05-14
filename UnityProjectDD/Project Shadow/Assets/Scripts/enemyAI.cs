using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] AIHealth health;


    [SerializeField] int HP;
    [SerializeField] int maxHP;
    [SerializeField] float shootRate;
    
    bool isShooting;
    bool playerInRange;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);

        health = GetComponentInChildren<AIHealth>();

        health.updateHealthBar(HP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
           agent.SetDestination(gameManager.instance.player.transform.position);

            if (!isShooting)
            {
                StartCoroutine(shoot());
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

    IEnumerator flashred()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.blue;
    }
}