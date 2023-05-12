using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // variables
    public GameObject player;
    
    [Header("Main Camera")]
    public Camera cameraToUse;
    
    [Header("GameObject to follow the mouse")]
    public GameObject gameObjectToBeRotated;
    public GameObject barrelOfTheTank;
    
    
    [Header("PlayerUIController Script")]
    [SerializeField] private PlayerUIController uiController;
        
    
    [Header("Player Stats")]
    public int maxHp = 100;
    public int currentHp;
    public int dmg = 20;
    public float firerate = 1.0f;
    public float moveSpeed = 5f;
    public int hpregen = 5;
    public int level;
    public int exp;
    public int statPoints;
    public int score;
    private bool isRegenHealth;
    public Transform firePoint;
    public float nextFireTime = 0f;
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    private bool damageTaken = false;

    [Header("Stats gained pr upgrade")]
    public int bonusHpPerLevel = 100;
    public int bonusDmgPerLevel = 20;
    public float bonusFireRatePerLevel = 1.0f;
    public float bonusMoveSpeed = 5f;
    public int bonusHpRegenPerLevel = 5;

    [Header("Class Specific Variables")] 
    public float bulletRadius;

    [Header("Player Info")]
    public string playerName;
    public string pinCode;
    
    public bool playerGotHit;
    private bool _playerDead = false;
    private Rigidbody2D _rb2D;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        uiController = GetComponent<PlayerUIController>();
        playerGotHit = false;
    }

    private void Update()
    {
        RotateToMouse2D();
        PlayerShoot();
        PlayerTakeDamage();
        CharacterMovement();
        RegenHealtOverTime();
        Hit();
    }

    private void FixedUpdate()
    {
        
    }
    
    
    // Methods
    
        // Main Methods
        //*******************************************************
        //
        // Det er disse metoder du skal implementere Erik :) 
        //
        //*******************************************************
        
        // Makes the player follow the mouse ( Use the objectToGetRotated gameobject as the object that should be rotated)
        // If you rotated the entire player objects, you will then also rotate the cameras
        
        void RotateToMouse2D()
        {
            Vector3 mousePosition = cameraToUse.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the angle between the mouse and the player's position
            Vector3 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the player to face the mouse
            player.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Makes the player regen health over time
        private void RegenHealtOverTime()
        {
            if (damageTaken == false)
            {
                if (currentHp != maxHp && !isRegenHealth)
                {
                    StartCoroutine("regeainHealthOverTime");
                }
            }
        }

    public void Damage(int damage)
    {
        if (damage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Damage cannot be negative"); // Since we dont want to be capable of dealing negative damage, i've made the script throw an error
        }

        currentHp -= damage; // This script is made to allow someone to damage this character
        if (currentHp <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator regeainHealthOverTime()
        {
            isRegenHealth = true;
            while (currentHp < maxHp)
            {
                currentHp++;
                yield return new WaitForSeconds(hpregen);
            }
            isRegenHealth = false;
        }

        // Controls the players movement ( PlayerMovement() er et ugyldigt navn for en metode. Derfor blev det CharacterMovement() )
        // Bare lige FYI :) 
        private void CharacterMovement()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (Input.GetKey(KeyCode.A))
                rb.AddForce(Vector3.left);
            if (Input.GetKey(KeyCode.D))
                rb.AddForce(Vector3.right);
            if (Input.GetKey(KeyCode.W))
                rb.AddForce(Vector3.up);
            if (Input.GetKey(KeyCode.S))
                rb.AddForce(Vector3.down);
        }   

        // Makes the player take damage if the collider with enemies or enemyteams bullets
        private void PlayerTakeDamage()
        {
        currentHp - dmg;
        haveTakenDamage();
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            PlayerTakeDamage();
        }
        else if(collision.gameObject.tag == "red" && player.tag == "blue")
        {
            PlayerTakeDamage();
        }
        else if(collision.gameObject.tag == "blue" && player.tag == "red")
        {
            PlayerTakeDamage();
        }
    }

    // Makes the player shoot
    private void PlayerShoot()
        {
        // Check if the fire button is pressed and if enough time has passed since the last shot
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            // Spawn a bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Set the speed of the bullet
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = firePoint.right * bulletSpeed;

            // Set the time of the next shot
            nextFireTime = Time.time + fireRate;
        }
    }

        // Pauses Health Regeneration when hit by an enemy
        private IEnumerator haveTakenDamage()
        {
            damageTaken = true;
            yield return new WaitForSeconds(5f);
        }

    //___________________________________________________

    // UI Methods

    public void HpUpgrade()
        {
            maxHp += bonusHpPerLevel;
            currentHp += bonusHpPerLevel;
            statPoints--;
        }

        public void DmgUpgrade()
        {
            dmg += bonusDmgPerLevel;
            statPoints--;
        }

        public void FirerateUpgrade()
        {
            firerate += bonusFireRatePerLevel;
            statPoints--;
        }

        public void MovespeedUpgrade()
        {
            moveSpeed += bonusMoveSpeed;
            statPoints--;
        }

        public void HpregenUpgrade()
        {
            hpregen += bonusHpPerLevel;
            statPoints--;
        }
    
        
        // ***********************************
        //
        // Denne metode bliver slettet senere. Den er lavet for at kunne teste om "kill" mechanicen fungere.
        // Jeg skal nok slette den når vi kommer dertil.
        //
        // ***********************************
        public void Hit()
        {
            if (Input.GetKeyDown(KeyCode.K) && playerGotHit)
            {
                Debug.Log("Dubble Hit");
                CancelInvoke();
                Invoke(nameof(HitReset),3);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                playerGotHit = true;
                if (currentHp <= 20)
                {
                    _playerDead = true;
                    uiController.DeathScreen();
                    gameObjectToBeRotated.layer = 8;
                    barrelOfTheTank.layer = 8;
                    _rb2D.velocity = new Vector2(0, 0);
                }
                currentHp = currentHp - 20;
                Debug.Log(currentHp);
                Invoke(nameof(HitReset), 3);
            }
        }
        void HitReset()
        {
            playerGotHit = false;
        }
}
