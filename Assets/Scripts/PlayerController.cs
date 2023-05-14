using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    // variables
    //public GameObject player;
    
    [Header("Cameras")]
    public Camera cameraToUse;
    public Camera miniMap;
    public Camera deathScreen;
    
    [Header("GameObject to follow the mouse")]
    public GameObject gameObjectToBeRotated;
    public GameObject barrelOfTheTank;
    
    
    [Header("PlayerUIController Script")]
    [SerializeField] private PlayerUIController uiController;
        
    
    [Header("Player Stats")]
    public int maxHp = 100;
    public int currentHp;
    public int dmg = 20;
    public float moveSpeed = 5f;
    public int hpRegen = 5;
    public int level;
    public int exp;
    public int statPoints;
    public int score;
    public float nextFireTime = 0f;
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
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
    public string teamName;
    public bool playerGotHit;
    private bool _playerDead = false;
    private Rigidbody2D _rb2D;

    [Space] 
    [Header("Prefabs & GameObjects:")] 
    public GameObject bulletPrefab;
    public Transform firePoint;
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        uiController = GetComponent<PlayerUIController>();
        playerGotHit = false;
        nextFireTime = fireRate;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            CmdRotateToMouse2D();
            CmdShoot();
            CharacterMovement();
            RegenHealthOverTime();
            Hit();
            
        }
        else
        {
            Debug.Log(transform.name + " is not local :( ", transform);
            cameraToUse.enabled = false;
            miniMap.enabled = false;
            deathScreen.enabled = false;
        }
        
    }

    private void FixedUpdate()
    {
        
    }
    
    
    // Methods
    
        // Main Methods
        [Command]
        void CmdRotateToMouse2D()
        {
            Vector3 mousePosition = cameraToUse.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the angle between the mouse and the player's position
            Vector3 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the player to face the mouse
            gameObjectToBeRotated.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        
        private void RegenHealthOverTime()
        {
            if (currentHp < maxHp && !playerGotHit)
            {
                currentHp += hpRegen * (int)Time.deltaTime; // Increase current health by regenRate per second
                currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Clamp current health to be within the range of 0 and max health
            }
        }
        
        private void CharacterMovement()
        {
            if (Input.GetKey(KeyCode.A))
                _rb2D.AddForce(Vector2.left * moveSpeed);
            if (Input.GetKey(KeyCode.D))
                _rb2D.AddForce(Vector2.right * moveSpeed);
            if (Input.GetKey(KeyCode.W))
                _rb2D.AddForce(Vector2.up * moveSpeed);
            if (Input.GetKey(KeyCode.S))
                _rb2D.AddForce(Vector2.down * moveSpeed);
        }   
        
        private void PlayerTakeDamage(int damage)
        {
            if (playerGotHit)
            {
                Debug.Log("Dubble Hit");
                CancelInvoke(nameof(HitReset));
            }
            else
            {
                playerGotHit = true;    
            }
            if (currentHp <= damage)
            {
                _playerDead = true;
                uiController.DeathScreen();
                gameObjectToBeRotated.layer = 8;
                barrelOfTheTank.layer = 8;
                _rb2D.velocity = new Vector2(0, 0);
            }
            currentHp = currentHp-damage;
            Debug.Log(currentHp);
            Invoke(nameof(HitReset), 3);
        }

        /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            PlayerTakeDamage(collision.gameObject.);
        }
        else if(collision.gameObject.CompareTag("RedTeam") && player.CompareTag("BlueTeam"))
        {
            PlayerTakeDamage();
        }
        else if(collision.gameObject.CompareTag("BlueTeam") && player.CompareTag("RedTeam"))
        {
            PlayerTakeDamage();
        }
    }*/

        // Makes the player shoot
        [Command]
        private void CmdShoot()
        {
            // Check if the fire button is pressed and if enough time has passed since the last shot
            if (Input.GetButtonDown("Fire1") && nextFireTime <= 0)
            {
            // Spawn a bullet at the fire point
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                bullet.tag = tag;
            // Set the speed of the bullet
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = firePoint.right * bulletSpeed;

            // Set the time of the next shot
            nextFireTime = fireRate;
            Destroy(bullet,3f);
            }
            else
            {
                nextFireTime -= Time.deltaTime;
            }
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
            fireRate += bonusFireRatePerLevel;
            statPoints--;
        }

        public void MovespeedUpgrade()
        {
            moveSpeed += bonusMoveSpeed;
            statPoints--;
        }

        public void HpregenUpgrade()
        {
            hpRegen += bonusHpPerLevel;
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
