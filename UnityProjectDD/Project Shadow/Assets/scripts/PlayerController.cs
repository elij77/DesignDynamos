using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class playerController : MonoBehaviour, IDamage, IHeal, IDefense
{

    [SerializeField] CharacterController playerControls;

    [SerializeField] int HP;

    [SerializeField] int Armor;

    [SerializeField] int speed;

    [SerializeField] int sprintMod;

    [SerializeField] int jumpMax;

    [SerializeField] int jumpSpeed;

    [SerializeField] int grav;

    [SerializeField] int shootDamage;

    [SerializeField] float shootRate;

    [SerializeField] int shootDist;

    [SerializeField] GameObject muzzleFlash;

    [SerializeField] int ammoCurr;
    [SerializeField] int ammoMax;
    [SerializeField] int clip;
    [SerializeField] int startup;

    [SerializeField] Transform shootPos;

    [SerializeField] float bulletSpeed;

    [SerializeField] Camera playerCamera;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    [SerializeField] GameObject gunModel;
    
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audJump;
    [SerializeField] AudioClip[] audhit;
    [SerializeField] AudioClip audReload;
    [SerializeField] AudioClip[] audFootSteps;
    [SerializeField] AudioClip audNoAmmo;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [Range(0, 1)][SerializeField] float audhitVol;
    [Range(0, 1)][SerializeField] float audReloadVol;
    [Range(0, 1)][SerializeField] float audFootVol;
    


    Vector3 moveDir;

    Vector3 playerVel;

    int jumpCount;

    int HPOrig;

    int ArmorOrig;

    int selectedGun;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        ArmorOrig = Armor;
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
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);

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
        if (HP > 0 && Armor <= 0)
        {
            HP -= amount;
        }
        else if (Armor > 0)
        {
            Armor -= amount;
        }
        updatePlayerUI();
        StartCoroutine(flashScreenDamage());
        aud.PlayOneShot(audhit[Random.Range(0, audhit.Length)], audhitVol);
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
        

       
    }

    public void RepairArmor(int amount)
    {
        if (Armor + amount > ArmorOrig)
        {
            Armor = ArmorOrig;
        }
        else
        {
            Armor += amount;
        }

        updatePlayerUI();
    }
    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerArmorBar.fillAmount = (float)Armor / ArmorOrig;
        if (gunList.Capacity > 0)
        {
            gameManager.instance.updateAmmo(gunList[selectedGun].ammoCurr, gunList[selectedGun].ammoMax);
        }
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

        aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootVolume);

        gunList[selectedGun].ammoCurr--;

        if (gunList[selectedGun].ammoCurr == 0)
        {
            aud.PlayOneShot(audNoAmmo, audReloadVol);
        }

        StartCoroutine(flashMuzzle());

        updatePlayerUI();

        //bullet2 = Instantiate(bullet, shootPos.position, Quaternion.identity);

        //Vector3 direction = playerCamera.transform.forward;

        //bullet2.transform.rotation = Quaternion.LookRotation(direction);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }

            Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
        }

        yield return new WaitForSeconds(shootRate);
        
        isShooting = false;
    }

    IEnumerator flashScreenDamage()
    {
        gameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameManager.instance.playerFlashDamage.SetActive(false);
    }

    IEnumerator flashMuzzle()
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.03f);

        muzzleFlash.SetActive(false);
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
        updatePlayerUI();
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

        updatePlayerUI();
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
        aud.PlayOneShot(audReload, audReloadVol);
        updatePlayerUI();
    }

    public void OnTriggerEnter(Collider other)
    {
        for (int j = 0; j < gunList.Count; j++)
        {
            if (other.gameObject.tag == "Ammo" && gunList.Capacity > 0 && gunList[j].ammoMax < gunList[j].startup)
            {
                for (int i = 0; i < gunList.Count; i++)
                {
                    gunList[i].ammoMax = gunList[i].startup;
                }
                Destroy(other.gameObject);
            }

            updatePlayerUI();
        }
    }
    //done
}
