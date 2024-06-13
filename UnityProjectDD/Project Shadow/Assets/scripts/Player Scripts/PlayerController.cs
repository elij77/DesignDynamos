using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage, IHeal, IDefense
{

    [SerializeField] CharacterController playerControls;

    /// Added current hp and max hp for level ups.Also added Exp fields.
  
    [SerializeField] int HP, currentHP, maxHP;
    [SerializeField] int currentExp, maxExp, currentLevel; // Added

    [SerializeField] int Armor;

    [SerializeField] int speed;
    [SerializeField] int walkSpeed;
    [SerializeField] int runSpeed;
    [SerializeField] float stam;
    [SerializeField] float stamMax;
    [SerializeField] float sprintAmount;
    [SerializeField] float jumpAmount;

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

    Vector3 moveDir;

    Vector3 playerVel;

    private Coroutine regen;

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

    // Added
    //private void OnEnable()
    //{
    //    ExperienceManager.instance.onExperienceChange += HandleExperienceChange;
    //}

    //private void OnDisable()
    //{
    //    ExperienceManager.instance.onExperienceChange -= HandleExperienceChange;
    //}

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
            if (Input.GetButton("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                stam -= sprintAmount * Time.deltaTime;
                if (stam < 0)
                {
                    stam = 0;
                }
                updatePlayerUI();

                if (regen != null)
                {
                    StopCoroutine(regen);
                }
                regen = StartCoroutine(Recharge());
            }
        if (Input.GetButtonDown("Reload") && gunList.Count > 0)
        {
            reload(gunList[selectedGun].clip);
        }
        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurr > 0 && !isShooting)
        {
            StartCoroutine(shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax && stam >= jumpAmount)
        {
            
            AudioManager.Instance.playJump("Jump1");
            jumpCount++;

            playerVel.y = jumpSpeed;

            stam -= jumpAmount;
            if (stam < 0)
            {
                stam = 0;
            }
            updatePlayerUI();

            if (regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(Recharge());
        }

        playerVel.y -= grav * Time.deltaTime;

        playerControls.Move(playerVel * Time.deltaTime);
        
    }

    void sprint()
    {
        
        if (Input.GetButton("Sprint"))
        {
            
            if (Input.GetButtonDown("Sprint"))
            {
                    speed = runSpeed;
            }
            if (stam == 0)
            {
                speed = walkSpeed;
            }
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed = walkSpeed;
        }
    }

    // Added
    public void HandleExperienceChange (int newExperience)
    {
        currentExp += newExperience;
        if (currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    // Added Level up
    private void LevelUp()
    {
        maxExp += 10;
        currentHP = maxHP;

        currentLevel++;

        currentExp = 0;

        maxExp += 100;
    }

    public void takeDamage(int amount)
    {
        if (HP > 0 && Armor <= 0)
        {
            HP -= amount;
             StartCoroutine(flashScreenDamage());
        }
        else if (Armor > 0)
        {
            Armor -= amount;
            StartCoroutine(flashScreenDamageBlue());
        }
        updatePlayerUI();
        
        AudioManager.Instance.playHit("Hit1");
        if (HP <= 0)
        {
            gameManager.instance.playerFlashDamage1.SetActive(false);
            gameManager.instance.playerFlashDamage2.SetActive(false);
            gameManager.instance.playerFlashDamage3.SetActive(false);
            gameManager.instance.playerFlashDamage4.SetActive(false);
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
        EraseBlood();
        gameManager.instance.ScreenFlashResetter();


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
        gameManager.instance.playerStamBar.fillAmount = stam / stamMax;
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

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(1f);

        while (stam < stamMax)
        {
            stam += stamMax / 15;
            if (stam > stamMax)
            {
                stam = stamMax;
            }
            updatePlayerUI();
            yield return new WaitForSeconds(.1f);
        }
        regen = null;
    }
    IEnumerator shoot()
    {
        isShooting = true;

        AudioManager.Instance.SFXSource.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootVolume);

        gunList[selectedGun].ammoCurr--;

        if (gunList[selectedGun].ammoCurr == 0)
        {
            AudioManager.Instance.playNoAmmo("AmmoClick");
        }

        StartCoroutine(flashMuzzle());

        updatePlayerUI();

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

    IEnumerator flashScreenDamageBlue()
    {
            gameManager.instance.playerFlashDamage.SetActive(true);
            yield return new WaitForSeconds(.5f);
            gameManager.instance.playerFlashDamage.SetActive(false);
    }

    IEnumerator flashScreenDamage()
    {
        if (HP % 5 == 4 && Armor == 0)
        {
            gameManager.instance.playerFlashDamage1.SetActive(true);
            yield return new WaitForSeconds(5f);
            gameManager.instance.playerFlashDamage1.SetActive(false);
        }
        else if (HP % 5 == 3 && Armor == 0)
        {
            gameManager.instance.playerFlashDamage2.SetActive(true);
            yield return new WaitForSeconds(5f);
            gameManager.instance.playerFlashDamage2.SetActive(false);
        }
        else if (HP % 5 == 2 && Armor == 0 )
        {
            gameManager.instance.playerFlashDamage3.SetActive(true);
            yield return new WaitForSeconds(5f);
            gameManager.instance.playerFlashDamage3.SetActive(false);
        }
        else if (HP % 5 == 1 && Armor == 0)
        {
            gameManager.instance.playerFlashDamage4.SetActive(true);
            if (HP % 5 > 1)
            {
                yield return new WaitForSeconds(3f);
                gameManager.instance.playerFlashDamage4.SetActive(false);
            }
            else
            {
                yield return new WaitForSeconds(50f);
                gameManager.instance.playerFlashDamage4.SetActive(false);
            }
        }
    }

    public void EraseBlood()
    {
        gameManager.instance.playerFlashDamage1.SetActive(false);
        gameManager.instance.playerFlashDamage2.SetActive(false);
        gameManager.instance.playerFlashDamage3.SetActive(false);
        gameManager.instance.playerFlashDamage4.SetActive(false);
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
        AudioManager.Instance.playSFX("Reload");
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
