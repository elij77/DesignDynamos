using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class playerController : MonoBehaviour, IDamage, IHeal
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

    [SerializeField] int ammoCurr;
    [SerializeField] int ammoMax;
    [SerializeField] int clip;
    [SerializeField] int startup;

    [SerializeField] Transform shootPos;

    [SerializeField] GameObject bullet;

    [SerializeField] GameObject bullet2;

    [SerializeField] float bulletSpeed;

    [SerializeField] Camera playerCamera;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    [SerializeField] GameObject gunModel;

    Vector3 moveDir;

    Vector3 playerVel;

    int jumpCount;

    int HPOrig;

    int selectedGun;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            swapGun();
        }

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
        if (Input.GetButtonDown("Reload") && gunList.Count > 0)
        {
            reload(gunList[selectedGun].clip);
        }
        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurr > 0 && !isShooting)
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
        updatePlayerUI();
        StartCoroutine(flashScreenDamage());

        if (HP <= 0)
        {
            gameManager.instance.loseMenu();
        }
    }
    public void Heal (int amount)
    {        
        if (HP + amount > HPOrig)
        {
            HP = HPOrig;
        }
        else
        {
        HP = HP + amount;
        }

        updatePlayerUI();
        StartCoroutine(flashScreenDamage());

       
    }
    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    public void spawnPlayer()
    {
        HP = HPOrig;
        updatePlayerUI();

        playerControls.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        playerControls.enabled = true;
        
    }
    IEnumerator shoot()
    {
        isShooting = true;

        gunList[selectedGun].ammoCurr--;

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

    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);

        selectedGun = gunList.Count - 1;

        shootDamage = gun.shootDamage;

        shootRate = gun.shootRate;

        shootDist = gun.shootDist;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gModel.GetComponent<MeshFilter>().sharedMesh;

        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gModel.GetComponent<MeshRenderer>().sharedMaterial;


        ammoMax = gun.ammoMax;
        clip = gun.clip;
        ammoCurr = gun.ammoCurr;
        startup = gun.startup;

    }

    void swapGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;

        shootDist = gunList[selectedGun].shootDist;

        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gModel.GetComponent<MeshFilter>().sharedMesh;

        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gModel.GetComponent<MeshRenderer>().sharedMaterial;


    }

   public void reload(int amount)
    {
        amount = gunList[selectedGun].clip - gunList[selectedGun].ammoCurr;
        if (gunList[selectedGun].ammoMax >= amount)
        {
            gunList[selectedGun].ammoCurr += amount;
            gunList[selectedGun].ammoMax -= amount;
        }
        else if (gunList[selectedGun].ammoMax < amount)
        {
            gunList[selectedGun].ammoCurr += gunList[selectedGun].ammoMax;
            gunList[selectedGun].ammoMax = 0;
        }
    }
    //done
}
