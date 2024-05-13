using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{

    [SerializeField] CharacterController playerControls;

    [SerializeField] int HP;

    [SerializeField] int speed;

    [SerializeField] int sprintMod;

    [SerializeField] int jumpMax;

    [SerializeField] int jumpSpeed;

    [SerializeField] int grav;

    [SerializeField] int shootDamage;

    [SerializeField] float shootRate;

    [SerializeField] int shootDist;

    [SerializeField] Transform shootPos;

    [SerializeField] GameObject bullet;

    [SerializeField] GameObject bullet2;

    [SerializeField] float bulletSpeed;

    [SerializeField] Camera playerCamera;

    Vector3 moveDir;

    Vector3 playerVel;

    int jumpCount;

    int HPOrig;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        if (playerControls.isGrounded)
        {
            jumpCount = 0;

            playerVel = Vector3.zero;
        }
        
        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                  (Input.GetAxis("Vertical") * transform.forward);

        playerControls.Move(moveDir * speed * Time.deltaTime);

        sprint();

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {

            jumpCount++;

            playerVel.y = jumpSpeed;

        }

        playerVel.y -= grav * Time.deltaTime;

        playerControls.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashScreenDamage());

        if (HP <= 0)
        {
            gameManager.instance.loseMenu();
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        bullet2 = Instantiate(bullet, shootPos.position, Quaternion.identity);

        Vector3 direction = playerCamera.transform.forward;

        bullet2.transform.rotation = Quaternion.LookRotation(direction);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;

    }

    IEnumerator flashScreenDamage()
    {
        gameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameManager.instance.playerFlashDamage.SetActive(false);
    }

    void UpdateUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
}
